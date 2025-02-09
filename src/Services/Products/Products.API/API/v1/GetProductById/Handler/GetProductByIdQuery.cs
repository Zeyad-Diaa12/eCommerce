namespace Products.API.API.v1.GetProductById.Handler;

public record GetproductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;