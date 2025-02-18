namespace Identity.Application.Handlers.RoleHandlers.CreateRole;

public class CreateRoleCommandHandler
    (IRoleService roleService)
    : ICommandHandler<CreateRoleCommand, CreateRoleResult>
{
    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await roleService.CreateRoleAsync(command.RoleName);
        return new CreateRoleResult(result);
    }
}
