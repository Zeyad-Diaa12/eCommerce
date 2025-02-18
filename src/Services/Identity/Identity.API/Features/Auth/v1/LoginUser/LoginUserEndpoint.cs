using Identity.Application.Handlers.AuthHandlers.LoginUser;

namespace Identity.API.Features.Auth.v1.LoginUser;

public class LoginUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/login",
        async (LoginUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginUserCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<LoginUserResponse>();

            return Results.Ok(response);
        })
        .WithName("LoginUser")
        .WithTags("Authorization")
        .Produces<LoginUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Login User")
        .WithDescription("Login User")
        .AllowAnonymous();
    }
}
