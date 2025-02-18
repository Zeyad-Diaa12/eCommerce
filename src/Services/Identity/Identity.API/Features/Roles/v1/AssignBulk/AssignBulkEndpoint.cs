using Identity.Application.Handlers.RoleHandlers.AssignBulk;

namespace Identity.API.Features.Roles.v1.AssignBulk;

public class AssignBulkEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/assign/bulk",
        async(AssignBulkRequest request, ISender sender) =>
        {
            var command = request.Adapt<AssignBulkCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<AssignBulkResponse>();
            return Results.Ok(response);
        })
        .WithName("AssignRoleToUsers")
        .WithTags("RoleManager")
        .Produces<AssignBulkResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Assign Role To Users in Bulk")
        .WithDescription("Assign Role To Users in Bulk")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
