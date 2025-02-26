namespace Identity.Application.Handlers.AuthHandlers.LogOut;

public class LogOutCommandHandler(IAuthService authService)
    : ICommandHandler<LogOutCommand, LogOutResult>
{
    public async Task<LogOutResult> Handle(
        LogOutCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.LogOutAsync(command.UserId);

        return new LogOutResult(result);
    }
}
