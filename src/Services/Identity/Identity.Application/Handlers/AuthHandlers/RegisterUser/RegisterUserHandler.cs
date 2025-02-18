namespace Identity.Application.Handlers.UserHandlers.RegisterUser;

public class RegisterUserHandler
    (IAuthService authService) 
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(command);

        return new RegisterUserResult(result);
    }
}
