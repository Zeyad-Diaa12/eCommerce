using Identity.Application.Handlers.RoleHandlers.GetUsersInRole;

namespace Identity.API.Features.Roles.v1.GetUsersInRole;

public class GetUsersInRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/roles/users/{name}",
        async (string name, ISender sender) =>
        {
            var result = await sender.Send(new GetUsersInRoleQuery(name));

            var response = result.Adapt<GetUsersInRoleResponse>();

            return Results.Ok(response);
        })
        .WithName("GetUsersInRole")
        .WithTags("RoleManager")
        .Produces<GetUsersInRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Users in Role")
        .WithDescription("Get Users in Role")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
