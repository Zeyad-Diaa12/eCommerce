namespace Identity.API.Features.Auth.v1.LoginUser;

public record LoginUserResponse(string Token, string RefreshToken, int ExpiresIn);
