namespace Identity.Application.Handlers.AuthHandlers.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : ICommand<RefreshTokenResult>;
