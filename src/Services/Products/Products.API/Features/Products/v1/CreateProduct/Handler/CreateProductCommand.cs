namespace Products.API.Features.Products.v1.CreateProduct.Handler;

public record CreateProductCommand(
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
) : ICommand<CreateProductResult>;
