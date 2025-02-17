using BuildingBlocks.CQRS.Command;
using Identity.Application.IServices;

namespace Identity.Application.Handlers.AuthHandlers.LoginUser;

public class LoginUserHandler
    (IAuthService authService)
    : ICommandHandler<LoginUserCommand, LoginUserResult>
{
    public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await authService.LogInAsync(command);

        return new LoginUserResult
        (
            Token: result.Token,
            RefreshToken: result.RefreshToken,
            ExpiresIn: result.ExpiresIn
        );
    }
}
