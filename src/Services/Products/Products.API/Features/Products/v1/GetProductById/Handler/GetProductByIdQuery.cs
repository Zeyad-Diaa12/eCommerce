namespace Products.API.Features.Products.v1.GetProductById.Handler;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;