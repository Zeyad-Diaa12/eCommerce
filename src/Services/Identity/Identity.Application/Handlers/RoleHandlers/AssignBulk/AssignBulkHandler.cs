
namespace Identity.Application.Handlers.RoleHandlers.AssignBulk;

public class AssignBulkCommandHandler
    (IRoleService roleService)
    : ICommandHandler<AssignBulkCommand, AssignBulkResult>
{
    public async Task<AssignBulkResult> Handle(AssignBulkCommand command, CancellationToken cancellationToken)
    {
        var result = await roleService.AssignUsersToRoleBulkAsync(command.UserIds, command.RoleName);
        return new AssignBulkResult(result);
    }
}
