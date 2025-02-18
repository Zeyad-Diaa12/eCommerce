namespace Identity.Application.Handlers.AuthHandlers.LoginUser;

public record LoginUserResult(
    string Token,
    string RefreshToken,
    int ExpiresIn
);
