namespace Products.API.Features.Products.v1.GetAllProducts.Handler;

public record GetProductsResult(
    IEnumerable<Product> Products
);
