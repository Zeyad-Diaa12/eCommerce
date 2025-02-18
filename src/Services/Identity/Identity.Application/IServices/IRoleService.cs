using Identity.Application.DTOs;

namespace Identity.Application.Services;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName);
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
    Task<bool> CheckRoleExistsAsync(string roleName);
    Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
    Task<IEnumerable<UserRoleResponse>> GetUsersInRoleAsync(string roleName);
    Task<bool> DeleteRoleAsync(string roleName);
    Task<bool> CheckUserHasRoleAsync(string userId, string roleName);
    Task<bool> AssignUsersToRoleBulkAsync(List<string> userIds, string roleName);
    Task<bool> RemoveUsersFromRoleBulkAsync(List<string> userIds, string roleName);
}