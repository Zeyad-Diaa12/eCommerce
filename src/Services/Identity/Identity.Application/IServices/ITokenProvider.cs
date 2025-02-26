using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Domain.Entites;

namespace Identity.Application.IServices;

public interface ITokenProvider
{
    Task<LoginUserResult> GenerateToken(User user);
    Task<bool> LogoutUser(string userId);
    Task<LoginUserResult> RefreshToken(string token, string refreshToken);
}
