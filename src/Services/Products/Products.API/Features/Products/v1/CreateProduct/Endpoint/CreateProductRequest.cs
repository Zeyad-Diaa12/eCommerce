namespace Products.API.Features.Products.v1.CreateProduct.Endpoint;

public record CreateProductRequest(
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
);
