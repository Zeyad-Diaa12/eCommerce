using BuildingBlocks.Exceptions;
using Identity.Application.Handlers.AuthHandlers.LoginUser;
using Identity.Application.Handlers.UserHandlers.RegisterUser;
using Identity.Application.IServices;
using Identity.Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Services;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole> roleManager,
    ITokenProvider tokenProvider
) : IAuthService
{
    public async Task<LoginUserResult> LogInAsync(LoginUserCommand command)
    {
        var user = await FindUserByIdentifierAsync(
            command.Email,
            command.Username,
            command.PhoneNumber
        );

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, false);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Wrong Password");
        }

        return await tokenProvider.GenerateToken(user);
    }

    public async Task<bool> LogOutAsync(string userId)
    {
        return await tokenProvider.LogoutUser(userId);
    }

    public async Task<LoginUserResult> RefreshTokenAsync(string token, string refreshToken)
    {
        return await tokenProvider.RefreshToken(token, refreshToken);
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
            PhoneNumber = command.PhoneNumber,
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");
        }

        return result.Succeeded;
    }

    #region Helpers
    private async Task<User?> FindUserByIdentifierAsync(
        string? email,
        string? username,
        string? phoneNumber
    )
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            var userByEmail = await userManager.FindByEmailAsync(email);
            if (userByEmail != null)
                return userByEmail;
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            var userByUsername = await userManager.FindByNameAsync(username);
            if (userByUsername != null)
                return userByUsername;
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var userByPhone = userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (userByPhone != null)
                return userByPhone;
        }

        return null;
    }
    #endregion
}
