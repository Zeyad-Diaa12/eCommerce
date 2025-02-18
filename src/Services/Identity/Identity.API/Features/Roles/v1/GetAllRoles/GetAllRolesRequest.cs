namespace Identity.API.Features.Roles.v1.GetAllRoles;

public record GetAllRolesRequest(
    int? PageNumber = 1,
    int? PageSize = 10
);
