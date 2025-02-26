using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Application.Handlers.UserHandlers.RegisterUser;

namespace Identity.Application.IServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserCommand command);
    Task<LoginUserResult> LogInAsync(LoginUserCommand command);
    Task<bool> LogOutAsync(string userId);
    Task<LoginUserResult> RefreshTokenAsync(string token, string refreshToken);
}
