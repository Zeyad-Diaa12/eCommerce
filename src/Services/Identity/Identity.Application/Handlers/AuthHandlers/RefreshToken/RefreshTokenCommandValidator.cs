namespace Identity.Application.Handlers.AuthHandlers.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Provide a Token");

        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Provide a Refresh Token");
    }
}
