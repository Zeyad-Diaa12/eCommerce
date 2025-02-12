namespace Products.API.Features.Products.v1.GetAllProducts.Endpoint;

public record GetProductsRequest(
    string? Search, 
    int? PageNumber = 1, 
    int? PageSize = 10
);