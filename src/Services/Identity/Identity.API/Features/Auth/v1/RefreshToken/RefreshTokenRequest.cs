namespace Identity.API.Features.Auth.v1.RefreshToken;

public record RefreshTokenRequest(string Token, string RefreshToken);
