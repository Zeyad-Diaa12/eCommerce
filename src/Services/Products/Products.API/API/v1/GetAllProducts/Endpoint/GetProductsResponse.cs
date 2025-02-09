namespace Products.API.API.v1.GetAllProducts.Endpoint;

public record GetProductsResponse(IEnumerable<Product> Products);