namespace Products.API.Features.Products.v1.CreateProduct.Handler;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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
    }
}
