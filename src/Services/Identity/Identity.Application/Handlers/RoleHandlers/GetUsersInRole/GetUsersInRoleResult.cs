using Identity.Application.DTOs;

namespace Identity.Application.Handlers.RoleHandlers.GetUsersInRole;

public record GetUsersInRoleResult(IEnumerable<UserRoleResponse> UsersInRole);
