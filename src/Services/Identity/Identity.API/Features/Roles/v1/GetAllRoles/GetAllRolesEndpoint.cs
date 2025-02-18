using Identity.Application.Handlers.RoleHandlers.GetAllRoles;

namespace Identity.API.Features.Roles.v1.GetAllRoles;

public class GetAllRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/roles", 
        async ([AsParameters] GetAllRolesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetAllRolesQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetAllRolesResponse>();
            return Results.Ok(response);
        })
        .WithName("GetAllRoles")
        .WithTags("RoleManager")
        .Produces<GetAllRolesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get All Roles")
        .WithDescription("Get All Roles")
        .RequireAuthorization("RequireSuperAdminRole");
    }
}
