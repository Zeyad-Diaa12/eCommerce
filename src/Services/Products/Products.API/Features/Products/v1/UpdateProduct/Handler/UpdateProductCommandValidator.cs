namespace Products.API.Features.Products.v1.UpdateProduct.Handler;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .MustAsync(async (productId, cancellationToken) =>
            {
                var product = await session.LoadAsync<Product>(productId, cancellationToken);
                return product != null;
            })
            .WithMessage("Product not found");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(10000)
            .WithMessage("Description must not exceed 10000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleForEach(x => x.Category)
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters")
            .Matches("^[a-zA-Z0-9 ]*$")
            .WithMessage("Category must contain only letters, numbers, or spaces.");

        RuleFor(x => x.ImageFile)
            .NotEmpty()
            .WithMessage("ImageFile is required");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock must be greater than or equal to 0");
    }
}
