namespace Products.API.API.v1.GetproductByCategory.Handler;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
