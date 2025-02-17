using Identity.Application.Handlers.UserHandlers.LoginUser;
using Identity.Application.Handlers.UserHandlers.RegisterUser;
using Identity.Application.IServices;
using Identity.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Services;

public class AuthService
    (UserManager<User> userManager, SignInManager<User> signInManager)
    : IAuthService
{
    public Task<LoginUserResult> LogInAsync(LoginUserCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterAsync(RegisterUserCommand command)
    {
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            UserName = command.Username,
            ProfilePictureUrl = "http://www.gravatar.com/avatar/?d=mp",
            CreatedOn = DateTime.UtcNow,
            PhoneNumber = command.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, command.Password);
        
        return result.Succeeded;
    }
}
