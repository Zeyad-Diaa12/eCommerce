using Identity.API.Features.Roles.v1.RemoveBulk;
using Identity.Application.Handlers.RoleHandlers.RemoveBulk;

namespace Identity.API.Features.Roles.v1.AssignBulk;

public class RemoveBulkEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/remove/bulk",
        async(RemoveBulkRequest request, ISender sender) =>
        {
            var command = request.Adapt<RemoveBulkCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RemoveBulkResponse>();
            return Results.Ok(response);
        })
        .WithName("RemoveRoleFromUsers")
        .WithTags("RoleManager")
        .Produces<RemoveBulkResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Remove Role from Users in Bulk")
        .WithDescription("Remove Role from Users in Bulk")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
