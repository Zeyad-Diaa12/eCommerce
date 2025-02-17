using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Domain.Entites;


namespace Identity.Application.IServices;

public interface ITokenProvider
{
    Task<LoginUserResult> GenerateToken(User user);
    Task<bool> RevokeToken(string token);
    Task<LoginUserResult> RefreshToken(string token, string refreshToken);
}
