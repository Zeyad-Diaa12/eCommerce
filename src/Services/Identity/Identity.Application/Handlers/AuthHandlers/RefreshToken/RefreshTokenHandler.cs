using Identity.Application.Handlers.AuthHandlers.LoginUser;

namespace Identity.Application.Handlers.AuthHandlers.RefreshToken;

public class RefreshTokenCommandHandler(IAuthService authService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.RefreshTokenAsync(command.Token, command.RefreshToken);

        return new RefreshTokenResult(
            Token: result.Token,
            RefreshToken: result.RefreshToken,
            ExpiresIn: result.ExpiresIn
        );
    }
}
