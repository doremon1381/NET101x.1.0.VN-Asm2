using IdentityModel;
using IdentityService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asm2
{
    public static class IdentityDbInitialize
    {
        /// <summary>
        /// seed admin user
        /// </summary>
        /// <param name="appBuilder"></param>
        public static void SeedUsers(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.CreateScope())
            {
                var identityServices = scope.ServiceProvider.GetRequiredService<IIdentityServices>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (userManager.Users.Count() == 0)
                {
                    // seed admin user
                    var admin = new ApplicationUser()
                    {
                        UserName = "admin",
                        Email = "doremon1381@gmail.com"
                    };

                    var result = userManager.CreateAsync(admin, "Admin@123").Result;
                    // at this step, admin role will be existed!
                    if (result.Succeeded)
                        _ = userManager.AddToRoleAsync(admin, UserRoles.ADMIN).Result;

                }
            }
        }

        public static void EnsureDbCreated(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                db.Database.EnsureCreated();
            }
        }

        public static async Task SeedRolesAsync(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var identityServices = scope.ServiceProvider.GetService<IIdentityServices>();

                // seed roles
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // create addmin role
                if (!await roleManager.RoleExistsAsync(UserRoles.ADMIN))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));

                // create patient role
                if (!roleManager.RoleExistsAsync(UserRoles.PATIENT).Result)
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.PATIENT));

                // create doctor role
                if (!roleManager.RoleExistsAsync(UserRoles.DOCTOR).Result)
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.DOCTOR));
            }
        }
    }
}
