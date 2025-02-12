namespace Products.API.Features.Products.v1.CreateProduct.Handler;

public class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Price = command.Price,
            ImageFile = command.ImageFile,
            Description = command.Description,
            Category = command.Category
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
