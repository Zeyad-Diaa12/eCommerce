using Identity.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Handlers.RoleHandlers.UpdateRole;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResult>
{
    private readonly IRoleService _roleService;
    public UpdateRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<UpdateRoleResult> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRoleAsync(command.OldRoleName, command.NewRoleName);
        return new UpdateRoleResult(result);
    }
}
