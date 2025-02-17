using Identity.Application.Services;
using MediatR;


namespace Identity.Application.Handlers.RoleHandlers.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResult>
{
    private readonly IRoleService _roleService;
    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _roleService.CreateRoleAsync(command.RoleName);
        return new CreateRoleResult(result);
    }
}
