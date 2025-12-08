// Dosya: FitnessCenterManagement/Controllers/Data/SeedData.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            // 1. Rollere Ekleme (Roles Seeding)
            await EnsureRole(roleManager, AdminRole);
            await EnsureRole(roleManager, MemberRole);

            // 2. Admin Kullanıcı Ekleme (Admin User Seeding)
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
                    EmailConfirmed = true // Email onaylı sayıyoruz
                };

                // Admin kullanıcısını oluştur ve şifresini ata
                var result = await userManager.CreateAsync(adminUser, AdminPassword);

                if (result.Succeeded)
                {
                    // Kullanıcıyı Admin rolüne ata
                    await userManager.AddToRoleAsync(adminUser, roleName);
                }
            }
        }
    }
}