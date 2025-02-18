using Identity.API.Features.Roles.v1.DeleteRole;
using Identity.Application.Handlers.RoleHandlers.RemoveRoleFromUser;

namespace Identity.API.Features.Roles.v1.RemoveRoleFromUser;

public class RemoveRoleFromUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/roles/remove",
        async (RemoveRoleFromUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RemoveRoleFromUserCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<RemoveRoleFromUserResponse>();

            return Results.Ok(result);
        })
        .WithName("RemoveRoleFromUser")
        .WithTags("RoleManager")
        .Produces<RemoveRoleFromUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("RemoveRoleFromUser")
        .WithDescription("RemoveRoleFromUser")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
