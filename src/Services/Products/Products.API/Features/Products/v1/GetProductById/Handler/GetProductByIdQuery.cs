namespace Products.API.Features.Products.v1.GetProductById.Handler;

public record GetproductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;