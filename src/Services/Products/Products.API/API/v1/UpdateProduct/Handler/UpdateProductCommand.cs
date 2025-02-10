namespace Products.API.API.v1.UpdateProduct.Handler;

public record UpdateProductCommand(
    Guid Id,
    decimal Price,
    string Name,
    string ImageFile,
    string Description,
    List<string> Category
) : ICommand<UpdateProductResult>;
