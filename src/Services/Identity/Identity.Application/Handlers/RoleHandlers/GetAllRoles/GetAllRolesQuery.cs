namespace Identity.Application.Handlers.RoleHandlers.GetAllRoles;

public record GetAllRolesQuery
(
    int? PageNumber = 1,
    int? PageSize = 10
) : IQuery<GetAllRolesResult>;
