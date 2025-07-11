using Asm2.Extensions;
using IdentityModel;
using IdentityService;
using MedicalModel;
using MedicalService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
                var medicalServices = scope.ServiceProvider.GetRequiredService<IMedicalServices>();

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

                    var adminPerson = new Person()
                    {
                        FirstName = "Tuan",
                        LastName = "Nguyen",
                        Email = "doremon1380@gmail.com",
                        PhoneNumber = "0987654321",
                        Address = "Unknown",
                        Gender = Gender.Male,
                        Nationality = NationCode.VIETNAM,
                        Roles = new List<PersonRole> { PersonRole.Patient }
                    };

                    medicalServices.AddPersonAsync(adminPerson).GetAwaiter().GetResult();
                }
            }
        }

        public static void EnsureIdentityDbCreated(this IApplicationBuilder appBuilder)
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
                if (!roleManager.RoleExistsAsync(UserRoles.USER).Result)
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            }
        }
    }
}
