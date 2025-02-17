using FluentValidation;
using Identity.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Handlers.RoleHandlers.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .NotEqual("SuperAdmin")
            .WithMessage("Role name cannot be empty or SuperAdmin")
            .MustAsync(async (name, cancellation) =>
            {
                return await roleManager.FindByNameAsync(name) == null;
            })
            .WithMessage("Role already exists");

    }
}
