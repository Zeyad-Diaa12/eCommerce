using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuildingBlocks.Cache;
using BuildingBlocks.Helpers;
using Identity.Application.DTOs;
using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Domain.Entites;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class TokenProvider : ITokenProvider
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly ICacheService _cache;

    public TokenProvider(
        IOptions<JwtSettings> jwtSettings,
        UserManager<User> userManager,
        ICacheService cache
    )
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _cache = cache;
    }

    public async Task<LoginUserResult> GenerateToken(User user)
    {
        var userId = user.Id.ToString();
        await InvalidateUserTokens(userId);

        var roles = await _userManager.GetRolesAsync(user);
        var claims = CreateClaims(user, roles);
        var token = CreateJwtToken(claims);
        var refreshToken = GenerateRefreshToken();
        var jti = GetJtiFromClaims(claims);

        await StoreUserSession(userId, jti, token, refreshToken);

        return new LoginUserResult(
            new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken,
            _jwtSettings.AccessTokenExpiration
        );
    }

    public async Task<bool> LogoutUser(string userId)
    {
        try
        {
            var sessionKey = GetUserSessionsKey(userId);
            var activeSessions =
                await _cache.GetAsync<List<UserSession>>(sessionKey) ?? new List<UserSession>();

            foreach (var session in activeSessions)
            {
                await BlacklistToken(session.Jti);
                await RemoveRefreshToken(userId, session.Jti);
            }

            await _cache.RemoveAsync(sessionKey);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<LoginUserResult> RefreshToken(string token, string refreshToken)
    {
        var principal = GetPrincipalFromToken(token);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        ValidateRefreshToken(userId, jti, refreshToken);
        var user = await GetUser(userId);

        await InvalidateOldTokens(userId, jti);
        return await GenerateToken(user);
    }

    private async Task StoreUserSession(
        string userId,
        string jti,
        JwtSecurityToken token,
        string refreshToken
    )
    {
        var sessionKey = GetUserSessionsKey(userId);
        var sessions =
            await _cache.GetAsync<List<UserSession>>(sessionKey) ?? new List<UserSession>();

        sessions.RemoveAll(s => s.Jti == jti);
        var newSession = CreateNewSession(jti, token, refreshToken);
        sessions.Add(newSession);

        await UpdateSessionStorage(userId, sessionKey, sessions, newSession);
    }

    private async Task UpdateSessionStorage(
        string userId,
        string sessionKey,
        List<UserSession> sessions,
        UserSession newSession
    )
    {
        await _cache.SetAsync(
            sessionKey,
            sessions,
            new DistributedCacheEntryOptions { AbsoluteExpiration = sessions.Max(s => s.ExpiresAt) }
        );

        await _cache.SetAsync(
            GetRefreshTokenKey(userId, newSession.Jti),
            newSession.RefreshToken,
            new DistributedCacheEntryOptions { AbsoluteExpiration = newSession.ExpiresAt }
        );
    }

    private async Task InvalidateUserTokens(string userId)
    {
        var sessionKey = GetUserSessionsKey(userId);
        var sessions =
            await _cache.GetAsync<List<UserSession>>(sessionKey) ?? new List<UserSession>();

        foreach (var session in sessions)
        {
            await BlacklistToken(session.Jti);
            await RemoveRefreshToken(userId, session.Jti);
        }

        await _cache.RemoveAsync(sessionKey);
    }

    private async Task ValidateRefreshToken(string userId, string jti, string refreshToken)
    {
        var storedToken = await _cache.GetAsync<string>(GetRefreshTokenKey(userId, jti));
        if (storedToken != refreshToken)
            throw new SecurityTokenException("Invalid refresh token");
    }

    private async Task InvalidateOldTokens(string userId, string oldJti)
    {
        await BlacklistToken(oldJti);
        await RemoveRefreshToken(userId, oldJti);
    }

    private async Task BlacklistToken(string jti)
    {
        await _cache.SetAsync(
            GetBlacklistKey(jti),
            "revoked",
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
                    _jwtSettings.AccessTokenExpiration
                ),
            }
        );
    }

    private async Task RemoveRefreshToken(string userId, string jti)
    {
        await _cache.RemoveAsync(GetRefreshTokenKey(userId, jti));
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),
            ClockSkew = TimeSpan.Zero,
        };

        var principal = handler.ValidateToken(token, validationParameters, out var securityToken);
        ValidateSession(principal, securityToken as JwtSecurityToken);
        return principal;
    }

    private void ValidateSession(ClaimsPrincipal principal, JwtSecurityToken jwtToken)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var jti = jwtToken?.Id;

        if (string.IsNullOrEmpty(jti))
            throw new SecurityTokenException("Invalid token");

        var sessionKey = GetUserSessionsKey(userId);
        var sessions = _cache.GetAsync<List<UserSession>>(sessionKey).Result;

        if (sessions?.Any(s => s.Jti == jti) != true)
            throw new SecurityTokenException("Invalid or expired session");
    }

    private async Task<User> GetUser(string userId)
    {
        return await _userManager.FindByIdAsync(userId)
            ?? throw new SecurityTokenException("User not found");
    }

    private List<Claim> CreateClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256
            )
        );
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GetJtiFromClaims(IEnumerable<Claim> claims) =>
        claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

    private UserSession CreateNewSession(string jti, JwtSecurityToken token, string refreshToken) =>
        new(
            jti,
            new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken,
            DateTime.UtcNow.AddSeconds(_jwtSettings.AccessTokenExpiration)
        );

    private string GetUserSessionsKey(string userId) => $"user_sessions:{userId}";

    private string GetRefreshTokenKey(string userId, string jti) =>
        $"refresh_tokens:{userId}:{jti}";

    private string GetBlacklistKey(string jti) => $"blacklisted_tokens:{jti}";
}
