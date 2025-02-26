namespace Identity.Application.Handlers.AuthHandlers.RefreshToken;

public record RefreshTokenResult(
    string Token,
    string RefreshToken,
    int ExpiresIn
);
