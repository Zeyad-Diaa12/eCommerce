namespace Identity.Application.Handlers.RoleHandlers.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
    public RemoveRoleFromUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User is required");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role is required");
    }
}
