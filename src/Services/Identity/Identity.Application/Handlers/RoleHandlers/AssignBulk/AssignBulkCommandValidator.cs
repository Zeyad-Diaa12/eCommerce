namespace Identity.Application.Handlers.RoleHandlers.AssignBulk;

public class AssignBulkCommandValidator : AbstractValidator<AssignBulkCommand>
{
    public AssignBulkCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role is required");

        RuleFor(x => x.UserIds)
            .NotEmpty()
            .WithMessage("Users is required");
    }
}
