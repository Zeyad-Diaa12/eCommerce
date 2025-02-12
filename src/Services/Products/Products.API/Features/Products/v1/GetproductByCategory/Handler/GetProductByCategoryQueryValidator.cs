namespace Products.API.Features.Products.v1.GetproductByCategory.Handler;

public class GetProductByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductByCategoryQueryValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.Category)
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters.");
        
        RuleFor(x => x.Category)
            .Matches("^[a-zA-Z0-9 ]*$")
            .WithMessage("Category must contain only letters, numbers, or spaces.");
    }
}
