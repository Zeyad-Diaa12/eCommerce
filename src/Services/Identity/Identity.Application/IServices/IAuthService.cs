using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Application.Handlers.UserHandlers.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.IServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserCommand command);
    Task<LoginUserResult> LogInAsync(LoginUserCommand command);
    Task<bool> LogOutAsync(string token);
    Task<LoginUserResult> RefreshTokenAsync(string token, string refreshToken);
}
