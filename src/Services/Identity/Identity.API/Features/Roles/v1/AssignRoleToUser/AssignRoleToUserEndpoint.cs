using Identity.Application.Handlers.RoleHandlers.AssignRoleToUser;

namespace Identity.API.Features.Roles.v1.AssignRoleToUser;

public class AssignRoleToUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/assign",
        async (AssignRoleToUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<AssignRoleToUserCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<AssignRoleToUserResponse>();
            return Results.Ok(response);
        })
        .WithName("AssignRoleToUser")
        .WithTags("RoleManager")
        .Produces<AssignRoleToUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Assign Role To User")
        .WithDescription("Assign Role To User")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
