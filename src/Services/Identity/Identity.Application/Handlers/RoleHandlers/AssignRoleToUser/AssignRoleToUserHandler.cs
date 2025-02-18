namespace Identity.Application.Handlers.RoleHandlers.AssignRoleToUser;

public class AssignRoleToUserCommandHandler
    (IRoleService roleService) 
    : ICommandHandler<AssignRoleToUserCommand, AssignRoleToUserResult>
{
    public async Task<AssignRoleToUserResult> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken)
    {
        var result = await roleService.AssignRoleToUserAsync(command.UserId, command.RoleName);
        return new AssignRoleToUserResult(result);
    }
}
