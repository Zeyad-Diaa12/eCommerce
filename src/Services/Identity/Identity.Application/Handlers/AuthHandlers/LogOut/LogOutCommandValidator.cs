namespace Identity.Application.Handlers.AuthHandlers.LogOut;

public class LogOutCommandValidator : AbstractValidator<LogOutCommand>
{
    public LogOutCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Provide a User Id");
    }
}
