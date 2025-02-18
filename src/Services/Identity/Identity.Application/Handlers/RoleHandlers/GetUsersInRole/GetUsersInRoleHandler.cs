namespace Identity.Application.Handlers.RoleHandlers.GetUsersInRole;

public class GetUsersInRoleHandler 
    (IRoleService roleService)
    : IQueryHandler<GetUsersInRoleQuery, GetUsersInRoleResult>
{
    public async Task<GetUsersInRoleResult> Handle(GetUsersInRoleQuery query, CancellationToken cancellationToken)
    {
        var users = await roleService.GetUsersInRoleAsync(query.RoleName);
        return new GetUsersInRoleResult(users);
    }
}
