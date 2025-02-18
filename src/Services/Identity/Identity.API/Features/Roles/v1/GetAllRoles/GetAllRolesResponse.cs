using Identity.Application.DTOs;

namespace Identity.API.Features.Roles.v1.GetAllRoles;

public record GetAllRolesResponse() : GetAllResponse<RoleResponse>;
