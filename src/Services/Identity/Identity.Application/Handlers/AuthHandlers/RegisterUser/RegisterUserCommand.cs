namespace Identity.Application.Handlers.UserHandlers.RegisterUser;

public record RegisterUserCommand(
    string FirstName, 
    string LastName, 
    string Email, 
    string Password,
    string ConfirmPassword,
    string Username,
    string PhoneNumber
) : ICommand<RegisterUserResult>;
