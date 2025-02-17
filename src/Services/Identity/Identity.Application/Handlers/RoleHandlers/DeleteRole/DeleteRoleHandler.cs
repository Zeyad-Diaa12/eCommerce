using Identity.Application.Services;
using MediatR;

namespace Identity.Application.Handlers.RoleHandlers.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResult>
{
    private readonly IRoleService _roleService;
    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<DeleteRoleResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRoleAsync(request.RoleName);
        return new DeleteRoleResult(result);
    }
}
