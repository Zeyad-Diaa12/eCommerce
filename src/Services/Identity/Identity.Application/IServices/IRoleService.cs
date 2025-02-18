using Identity.Application.DTOs;
using Identity.Application.Handlers.RoleHandlers.GetAllRoles;

namespace Identity.Application.Services;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName);
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
    Task<bool> CheckRoleExistsAsync(string roleName);
    Task<GetAllRolesResult> GetAllRolesAsync(int pageNumber, int pageSize);
    Task<IEnumerable<UserRoleResponse>> GetUsersInRoleAsync(string roleName);
    Task<bool> DeleteRoleAsync(string roleName);
    Task<bool> CheckUserHasRoleAsync(string userId, string roleName);
    Task<bool> AssignUsersToRoleBulkAsync(List<string> userIds, string roleName);
    Task<bool> RemoveUsersFromRoleBulkAsync(List<string> userIds, string roleName);
}