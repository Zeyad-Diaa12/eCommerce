namespace Products.API.Features.Products.v1.DeleteProduct.Handler;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        RuleFor(x => x.Id).
            NotEmpty().
            WithMessage("Id is required.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                var product = await session.LoadAsync<Product>(id, cancellationToken);
                return product != null;
            })
            .WithMessage("Product not found.");
    }
}
