namespace Identity.Application.Handlers.RoleHandlers.DeleteRole;

public class DeleteRoleCommandHandler
    (IRoleService roleService)
    : ICommandHandler<DeleteRoleCommand, DeleteRoleResult>
{
    public async Task<DeleteRoleResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.DeleteRoleAsync(request.RoleName);
        return new DeleteRoleResult(result);
    }
}
