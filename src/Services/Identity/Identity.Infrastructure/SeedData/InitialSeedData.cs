using Identity.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Identity.Infrastructure.SeedData;

public static class InitialSeedData
{
    internal static async Task SeedAsync(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<SeedData> logger)
    {
        try
        {
            await SeedRoles(roleManager, logger);
            await SeedSuperAdmin(userManager, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding initial data");
            throw;
        }
    }

    private static async Task SeedRoles(
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        var roles = new List<string> { "SuperAdmin", "Admin", "User" };

        foreach (var roleName in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                logger.LogInformation("Created {Role} role", roleName);
            }
        }
    }

    private static async Task SeedSuperAdmin(
        UserManager<User> userManager,
        ILogger logger)
    {
        const string superAdminEmail = "superadmin@admin.com";
        const string superAdminUserName = "superadmin";
        const string password = "SuperAdmin@2012004";

        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
        if (superAdminUser == null)
        {
            superAdminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = superAdminEmail,
                UserName = superAdminUserName,
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ProfilePictureUrl = "http://www.gravatar.com/avatar/?d=mp",
                CreatedOn = DateTime.UtcNow,
                PhoneNumber = "+201111676128"
            };

            var createResult = await userManager.CreateAsync(superAdminUser, password);
            if (!createResult.Succeeded)
            {
                throw new Exception($"SuperAdmin user creation failed: {string.Join(", ", createResult.Errors)}");
            }

            logger.LogInformation("Created SuperAdmin user");
        }

        if (!await userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
        {
            await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            logger.LogInformation("Added SuperAdmin role to SuperAdmin user");
        }
    }
}