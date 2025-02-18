using Identity.Application.DTOs;

namespace Identity.Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        try
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }
        catch (Exception ex) 
        {
            throw new InternalServerException(ex.Message);
        }
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        if(roleName == "SuperAdmin")
        {
            throw new ValidationException("Cannot assign SuperAdmin role to user");
        }

        var user = await GetUserByIdAsync(userId);

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            throw new NotFoundException($"Role '{roleName}' does not exist");
        }

        if (await _userManager.IsInRoleAsync(user, roleName))
        {
            throw new BadRequestException($"User already has role '{roleName}'");
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var currentRole = userRoles.FirstOrDefault();

        await _userManager.RemoveFromRoleAsync(user, currentRole!);

        var result = await _userManager.AddToRoleAsync(user, roleName);
        
        return result.Succeeded;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
    {
        var user = await GetUserByIdAsync(userId);
        if(roleName == "SuperAdmin")
        {
            throw new ValidationException("Cannot remove SuperAdmin role from user");
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            throw new ValidationException($"User does not have role '{roleName}'");
        }

        if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
        {
            throw new ValidationException("Cannot modify roles for Super Admin Users");
        }
        
        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        
        await _userManager.AddToRoleAsync(user, "User");

        return result.Succeeded;
    }

    public async Task<bool> CheckRoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList();

        return roles;
    }

    public async Task<IEnumerable<UserRoleResponse>> GetUsersInRoleAsync(string roleName)
    {
        return await _userManager.GetUsersInRoleAsync(roleName)
            .ContinueWith(task => task.Result.Select(u =>
                new UserRoleResponse(
                    u.Id.ToString(),
                    u.UserName,
                    u.Email,
                    roleName
                )
            ));
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            throw new NotFoundException($"Role '{roleName}' not found");
        }

        if (role.Name == "SuperAdmin")
        {
            throw new ValidationException("Cannot delete Super Admin role");
        }

        var users = await GetUsersInRoleAsync(roleName);

        if (users.Count() > 0)
        {
            throw new ValidationException($"Cannot delete role '{roleName}' with assigned users");
        }

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> CheckUserHasRoleAsync(string userId, string roleName)
    {
        var user = await GetUserByIdAsync(userId);
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await GetUserByIdAsync(userId);
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> AssignUsersToRoleBulkAsync(List<string> userIds, string roleName)
    {
        ValidateUserIds(userIds);

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            throw new NotFoundException($"Role '{roleName}' not found");
        }

        if(roleName == "SuperAdmin")
        {
            throw new ValidationException("Cannot assign users to SuperAdmin role");
        }

        var users = new List<User>();
        var errors = new List<string>();

        foreach (var userId in userIds)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    errors.Add($"User {userId} already has role '{roleName}'");
                    continue;
                }
                users.Add(user);
            }
            catch (NotFoundException ex)
            {
                errors.Add(ex.Message);
            }
        }

        if (errors.Any())
        {
            throw new BadRequestException($"Validation errors:\n{string.Join("\n", errors)}");
        }

        foreach (var user in users)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new InternalServerException($"Failed to assign role to user {user.Id}");
            }
        }

        return true;
    }

    public async Task<bool> RemoveUsersFromRoleBulkAsync(List<string> userIds, string roleName)
    {
        ValidateUserIds(userIds);

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            throw new NotFoundException($"Role '{roleName}' not found");
        }

        var users = new List<User>();
        var errors = new List<string>();

        foreach (var userId in userIds)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);

                if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    throw new ValidationException($"Cannot modify roles for SuperAdmin user {userId}");
                }

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    errors.Add($"User {userId} doesn't have role '{roleName}'");
                    continue;
                }
                users.Add(user);
            }
            catch (NotFoundException ex)
            {
                errors.Add(ex.Message);
            }
        }

        if (errors.Any())
        {
            throw new ValidationException($"Validation errors:\n{string.Join("\n", errors)}");
        }

        foreach (var user in users)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new InternalServerException($"Failed to remove role from user {user.Id}");
            }
        }

        return true;
    }

    #region Helpers
    private async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user ?? throw new NotFoundException($"User with ID '{userId}' not found");
    }
    private void ValidateUserIds(List<string> userIds)
    {
        if (userIds == null || !userIds.Any())
        {
            throw new ValidationException("At least one user must be specified");
        }

        if (userIds.Count > 1000)
        {
            throw new ValidationException("Maximum 1000 users per bulk operation");
        }
    }

    #endregion
}