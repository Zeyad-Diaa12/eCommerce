using Identity.API.Features.Roles.v1.CreateRole;
using Identity.Application.Handlers.RoleHandlers.DeleteRole;

namespace Identity.API.Features.Roles.v1.DeleteRole;

public class DeleteRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/roles/delete/{name}",
        async (string name, ISender sender) =>
        {
            var result = await sender.Send(new DeleteRoleCommand(name));
            
            var response = result.Adapt<DeleteRoleResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteRole")
        .WithTags("RoleManager")
        .Produces<DeleteRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Role")
        .WithDescription("Delete Role")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
