namespace Products.API.Features.Products.v1.GetAllProducts.Handler;

public record GetProductsQuery
(
    int? PageNumber = 1, 
    int? PageSize = 10
) : IQuery<GetProductsResult>;