namespace Identity.Application.Handlers.RoleHandlers.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandHandler 
    (IRoleService roleService)
    : ICommandHandler<RemoveRoleFromUserCommand, RemoveRoleFromUserResult>
{
    public async Task<RemoveRoleFromUserResult> Handle(RemoveRoleFromUserCommand command, CancellationToken cancellationToken)
    {
        var result = await roleService.RemoveRoleFromUserAsync(command.UserId, command.RoleName);

        return new RemoveRoleFromUserResult(result);
    }
}
