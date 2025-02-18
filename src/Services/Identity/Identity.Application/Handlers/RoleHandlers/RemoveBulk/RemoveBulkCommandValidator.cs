namespace Identity.Application.Handlers.RoleHandlers.RemoveBulk;

public class RemoveBulkCommandValidator : AbstractValidator<RemoveBulkCommand>
{
    public RemoveBulkCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role is required");

        RuleFor(x => x.UserIds)
            .NotEmpty()
            .WithMessage("Users is required");
    }
}
