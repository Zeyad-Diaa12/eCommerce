using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Handlers.RoleHandlers.AssignRoleToUser;

public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var roleService = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role Name is required");
    }
}
