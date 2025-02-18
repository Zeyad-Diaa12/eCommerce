namespace Identity.Application.Handlers.RoleHandlers.DeleteRole;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required")
            .MustAsync(async (roleName, cancellation) =>
            {
                var role = await roleManager.FindByNameAsync(roleName);
                return role != null;
            })
            .WithMessage("Role does not exist");
    }
}
