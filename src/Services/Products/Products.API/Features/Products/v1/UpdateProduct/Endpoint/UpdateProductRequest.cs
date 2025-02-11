namespace Products.API.Features.Products.v1.UpdateProduct.Endpoint;

public record UpdateProductRequest(
    Guid Id,
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
);
