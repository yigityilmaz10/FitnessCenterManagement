// Dosya: Controllers/Data/SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace FitnessCenterManagement.Data
{
    public static class SeedData
    {
        // Ödev gereksinimi: Admin E-posta ve Şifre
        private const string AdminEmail = "ogrencinumarasi@sakarya.edu.tr";
        private const string AdminPassword = "sau";
        private const string AdminRole = "Admin";
        private const string MemberRole = "Member";

        public static async Task Initialize(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Rolleri Oluşturma
            await EnsureRole(roleManager, AdminRole);
            await EnsureRole(roleManager, MemberRole);

            // 2. Admin Kullanıcıyı Oluşturma ve Role Atama
            await EnsureAdminUser(userManager, AdminRole);
        }

        private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task EnsureAdminUser(UserManager<IdentityUser> userManager, string roleName)
        {
            var adminUser = await userManager.FindByEmailAsync(AdminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, AdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, roleName);
                }
            }
        }
    }
}