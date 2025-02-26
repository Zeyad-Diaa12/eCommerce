namespace Identity.API.Features.Auth.v1.RefreshToken;

public record RefreshTokenResponse(string Token, string RefreshToken, int ExpiresIn);
