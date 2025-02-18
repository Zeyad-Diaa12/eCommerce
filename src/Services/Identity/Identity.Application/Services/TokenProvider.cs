using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class TokenProvider : ITokenProvider
{
    private readonly JwtSettings _jwtOptions;
    private readonly UserManager<User> _userManager;
    //private readonly IDistributedCache _cache;

    public TokenProvider(
        IOptions<JwtSettings> jwtOptions,
        UserManager<User> userManager)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
    }

    public async Task<LoginUserResult> GenerateToken(User user)
    {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = CreateClaims(user, roles);
            var token = CreateJwtToken(claims);
            var refreshToken = GenerateRefreshToken();

            //await StoreRefreshToken(user.Id, GetJtiFromClaims(claims), refreshToken);

        
            var tokenDate = token.ValidTo;
            var currentDate = DateTime.UtcNow;

            var totalMinutes = (int)(tokenDate - currentDate).TotalMinutes;

            return new LoginUserResult
            (
                new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken,
                totalMinutes
            );
    }

    public async Task<bool> RevokeToken(string token)
    {
        var principal = GetPrincipalFromToken(token, validateLifetime: true);
        if (principal == null) return false;

        var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        //await BlacklistToken(jti);
        //await RemoveRefreshToken(userId, jti);

        return true;
    }

    public async Task<LoginUserResult> RefreshToken(string token, string refreshToken)
    {
        var principal = GetPrincipalFromToken(token, validateLifetime: false);
        if (principal == null)
            throw new SecurityTokenException("Invalid token");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        //if (await IsTokenBlacklisted(jti))
        //    throw new SecurityTokenException("Token revoked");

        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new SecurityTokenException("User not found");

        //if (!await ValidateRefreshToken(userId, jti, refreshToken))
        //    throw new SecurityTokenException("Invalid refresh token");

        //await InvalidateOldTokens(userId, jti);

        return await GenerateToken(user);
    }

    //private async Task<bool> ValidateRefreshToken(string userId, string jti, string refreshToken)
    //{
    //    var storedToken = await _cache.GetStringAsync($"refresh_tokens:{userId}:{jti}");
    //    return storedToken == refreshToken;
    //}

    //private async Task InvalidateOldTokens(string userId, string oldJti)
    //{
    //    await BlacklistToken(oldJti);
    //    await RemoveRefreshToken(userId, oldJti);
    //}

    private List<Claim> CreateClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new("emailConfirmed", $"{user.EmailConfirmed}"),
            new("fullName", $"{user.FullName}")
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiration),
            signingCredentials: credentials);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    //private async Task StoreRefreshToken(string userId, string jti, string refreshToken)
    //{
    //    await _cache.SetStringAsync(
    //        $"refresh_tokens:{userId}:{jti}",
    //        refreshToken,
    //        new DistributedCacheEntryOptions
    //        {
    //            AbsoluteExpirationRelativeToNow =
    //                TimeSpan.FromDays(_jwtOptions.RefreshTokenExpiration)
    //        });
    //}

    //private async Task RemoveRefreshToken(string userId, string jti)
    //{
    //    await _cache.RemoveAsync($"refresh_tokens:{userId}:{jti}");
    //}

    //private async Task BlacklistToken(string jti)
    //{
    //    await _cache.SetStringAsync(
    //        $"blacklisted_tokens:{jti}",
    //        "revoked",
    //        new DistributedCacheEntryOptions
    //        {
    //            AbsoluteExpirationRelativeToNow =
    //                TimeSpan.FromMinutes(_jwtOptions.AccessTokenExpiration)
    //        });
    //}

    //private async Task<bool> IsTokenBlacklisted(string jti)
    //{
    //    return await _cache.GetStringAsync($"blacklisted_tokens:{jti}") != null;
    //}

    private ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Secret))
        }, out _);

        return principal;
    }

    private static string GetJtiFromClaims(IEnumerable<Claim> claims)
    {
        return claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
    }
}