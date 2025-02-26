using System.Security.Claims;
using Identity.Application.Handlers.AuthHandlers.LogOut;

namespace Identity.API.Features.Auth.v1.LogOut;

public class LogOutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/v1/auth/logout",
                async (HttpContext context, ISender sender) =>
                {
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(userId))
                    {
                        return Results.BadRequest("Invalid user");
                    }

                    var result = await sender.Send(new LogOutCommand(userId));

                    var response = result.Adapt<LogOutResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("LogOutUser")
            .WithTags("Authorization")
            .Produces<LogOutResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("LogOut User")
            .WithDescription("LogOut User")
            .RequireAuthorization("GeneralAccess");
    }
}
