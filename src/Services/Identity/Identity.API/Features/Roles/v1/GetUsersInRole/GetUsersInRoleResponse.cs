using Identity.Application.DTOs;

namespace Identity.API.Features.Roles.v1.GetUsersInRole;

public record GetUsersInRoleResponse(IEnumerable<UserRoleResponse> UsersInRole);
