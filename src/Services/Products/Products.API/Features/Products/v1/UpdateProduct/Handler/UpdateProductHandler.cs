namespace Products.API.Features.Products.v1.UpdateProduct.Handler;

public class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.ProductId, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Price = command.Price;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Stock = command.Stock;

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
