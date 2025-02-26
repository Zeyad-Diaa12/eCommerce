namespace Identity.Application.DTOs;

public record UserSession(string Jti, string AccessToken, string RefreshToken, DateTime ExpiresAt);
