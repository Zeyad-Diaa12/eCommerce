namespace Products.API.Features.Products.v1.DeleteProduct.Handler;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
