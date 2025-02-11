namespace Products.API.API.v1.DeleteProduct.Handler;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
