using Identity.Application.IServices;
using Identity.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers.UserHandlers.RegisterUser;

public class RegisterUserHandler
    (IAuthService authService) 
    : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(command);

        return new RegisterUserResult(result);
    }
}
