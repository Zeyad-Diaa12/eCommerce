namespace Products.API.API.v1.GetAllProducts.Handler;

public record GetProductsResult(
    IEnumerable<Product> Products
);
