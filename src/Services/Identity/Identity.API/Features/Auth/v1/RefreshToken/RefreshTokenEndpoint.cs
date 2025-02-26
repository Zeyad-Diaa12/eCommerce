using Identity.API.Features.Auth.v1.LoginUser;
using Identity.Application.Handlers.AuthHandlers.RefreshToken;

namespace Identity.API.Features.Auth.v1.RefreshToken;

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/v1/auth/refreshToken",
                async (RefreshTokenRequest request, ISender sender) =>
                {
                    var command = request.Adapt<RefreshTokenCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<RefreshTokenResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("RefreshToken")
            .WithTags("Authorization")
            .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Refresh Token")
            .WithDescription("Refresh Token")
            .RequireAuthorization("GeneralAccess");
    }
}
