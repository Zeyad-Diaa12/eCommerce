namespace Products.API.Features.Products.v1.GetproductByCategory.Handler;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
