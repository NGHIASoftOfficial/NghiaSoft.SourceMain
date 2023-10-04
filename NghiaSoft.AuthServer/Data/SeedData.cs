using Microsoft.AspNetCore.Identity;
using NghiaSoft.Core;

namespace NghiaSoft.AuthServer.Data;

public static class SeedData
{
    public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(Constants.BaseSeedData.AdminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(Constants.BaseSeedData.AdminRoleName));
        }
    }

    private static async Task SeedUsers(UserManager<IdentityUser> userManager)
    {
        if (await userManager.FindByEmailAsync(Constants.BaseSeedData.AdminUserEmail) == null)
        {
            var user = new IdentityUser
            {
                UserName = Constants.BaseSeedData.AdminUserName,
                Email = Constants.BaseSeedData.AdminUserEmail
            };

            var result = await userManager.CreateAsync(user, Constants.BaseSeedData.AdminUserPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Constants.BaseSeedData.AdminRoleName);
            }
        }
    }
}