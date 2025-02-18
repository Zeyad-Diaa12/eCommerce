using Identity.Application.Handlers.RoleHandlers.CreateRole;

namespace Identity.API.Features.Roles.v1.CreateRole;

public class CreateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/create",
        async (CreateRoleRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateRoleCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateRoleResponse>();
            return Results.Created($"/api/v1/roles", response);
        })
        .WithName("CreateRole")
        .WithTags("RoleManager")
        .Produces<CreateRoleResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Role")
        .WithDescription("Create Role")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
