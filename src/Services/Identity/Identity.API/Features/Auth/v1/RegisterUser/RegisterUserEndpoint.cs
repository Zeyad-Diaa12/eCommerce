using Carter;
using Identity.Application.Handlers.UserHandlers.RegisterUser;
using Mapster;
using MediatR;

namespace Identity.API.Features.Auth.v1.RegisterUser;

public class RegisterUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/register", 
        async (RegisterUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterUserCommand>();
            
            var result = await sender.Send(command);

            var response = result.Adapt<RegisterUserResponse>();

            return Results.Created($"/api/v1/auth/register", response);
        })
        .WithName("RegisterUser")
        .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register User")
        .WithDescription("Register User")
        .AllowAnonymous();
    }
}
