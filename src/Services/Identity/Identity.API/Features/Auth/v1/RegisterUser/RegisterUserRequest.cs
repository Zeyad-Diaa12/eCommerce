namespace Identity.API.Features.Auth.v1.RegisterUser;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string Username,
    string PhoneNumber
);
