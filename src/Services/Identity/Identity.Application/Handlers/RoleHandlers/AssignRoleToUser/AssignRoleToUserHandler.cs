using BuildingBlocks.CQRS.Command;
using Identity.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Handlers.RoleHandlers.AssignRoleToUser;

public class AssignRoleToUserCommandHandler : ICommandHandler<AssignRoleToUserCommand, AssignRoleToUserResult>
{
    private readonly IRoleService _roleService;

    public AssignRoleToUserCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<AssignRoleToUserResult> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _roleService.AssignRoleToUserAsync(command.UserId, command.RoleName);
        return new AssignRoleToUserResult(result);
    }
}
