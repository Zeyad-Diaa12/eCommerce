namespace Identity.Application.Handlers.RoleHandlers.RemoveBulk;

public class RemoveBulkHandler 
    (IRoleService roleService)
    : ICommandHandler<RemoveBulkCommand, RemoveBulkResult>
{
    public async Task<RemoveBulkResult> Handle(RemoveBulkCommand command, CancellationToken cancellationToken)
    {
        var result = await roleService.RemoveUsersFromRoleBulkAsync(command.UserIds, command.RoleName);
        return new RemoveBulkResult(result);
    }
}
