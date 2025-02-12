namespace Products.API.Features.Products.v1.UpdateProduct.Handler;

public record UpdateProductCommand(
    Guid ProductId,
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
) : ICommand<UpdateProductResult>;
