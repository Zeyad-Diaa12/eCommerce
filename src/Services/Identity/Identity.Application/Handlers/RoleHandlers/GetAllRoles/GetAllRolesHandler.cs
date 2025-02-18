
namespace Identity.Application.Handlers.RoleHandlers.GetAllRoles;

public class GetAllRolesQueryHandler 
    (IRoleService roleService)
    : IQueryHandler<GetAllRolesQuery, GetAllRolesResult>
{
    public Task<GetAllRolesResult> Handle(GetAllRolesQuery query, CancellationToken cancellationToken)
    {
        var result = roleService.GetAllRolesAsync(query.PageNumber??1, query.PageSize??10);

        return result;
    }
}
