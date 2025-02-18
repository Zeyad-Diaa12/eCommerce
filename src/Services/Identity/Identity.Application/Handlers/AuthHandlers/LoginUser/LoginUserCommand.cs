namespace Identity.Application.Handlers.AuthHandlers.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password,
    string Username,
    string PhoneNumber
) : ICommand<LoginUserResult>;
