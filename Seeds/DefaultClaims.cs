using FirstWebApplication.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FirstWebApplication.Seeds
{
    public static class DefaultClaims
    {
        public static async Task SeedClaimsForSuperAdmin(RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            if (adminRole != null)
            {
                await SeedPermissionClaims(roleManager, adminRole, "Products");
                // Add more permissions as needed
            }
            else
            {
                // Handle the case where the "SuperAdmin" role doesn't exist
                // You may log an error or take other appropriate action
            }
        }
        public static async Task SeedClaimsForUser(RoleManager<IdentityRole> roleManager)
        {
            var userRole = await roleManager.FindByNameAsync("User");
            if (userRole != null)
            {
                await SeedPermissionClaims(roleManager, userRole, "Products");
                // Add more permissions as needed
            }
            else
            {
                // Handle the case where the "SuperAdmin" role doesn't exist
                // You may log an error or take other appropriate action
            }
        }

        private async static Task SeedPermissionClaims(RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }

}
