namespace Products.API.Features.Products.v1.GetAllProducts.Endpoint;

public record GetProductsResponse(IEnumerable<Product> Products);