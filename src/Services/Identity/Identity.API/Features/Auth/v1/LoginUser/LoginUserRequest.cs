namespace Identity.API.Features.Auth.v1.LoginUser;

public record LoginUserRequest(
    string Email,
    string Password,
    string Username,
    string PhoneNumber
);