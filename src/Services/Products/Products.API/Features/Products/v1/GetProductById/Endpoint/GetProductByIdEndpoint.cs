using Products.API.Features.Products.v1.GetProductById.Handler;

namespace Products.API.Features.Products.v1.GetProductById.Endpoint;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetproductByIdQuery(id));

            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product by ID")
        .WithDescription("Get Product by ID");
    }
}
