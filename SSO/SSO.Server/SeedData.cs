namespace SSO.Server
{
    using Domain.Entities;
    using Domain.EntityFramework;
    using IdentityModel;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AuthDbContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var role = roleManager.FindByNameAsync("auth.admin").Result;

                if (role == null)
                {
                    var createRoleResult = roleManager.CreateAsync(new IdentityRole("auth.admin")).Result;

                    Console.WriteLine("auth.admin role created");
                }
                else
                {
                    Console.WriteLine("auth.admin role already exists");
                }

                var adminUser = userManager.FindByNameAsync("admin").Result;

                if (adminUser == null)
                {
                    var addRoleUser = userManager.AddToRoleAsync(adminUser, "auth.admin").Result;

                    adminUser = new ApplicationUser
                    {
                        UserName = "admin"
                    };

                    var result = userManager.CreateAsync(adminUser, "1071$1453#").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userManager.AddClaimsAsync(adminUser, new System.Security.Claims.Claim[]{
                        new System.Security.Claims.Claim(JwtClaimTypes.Name, "Admin"),
                        new System.Security.Claims.Claim(JwtClaimTypes.GivenName, "Admin"),
                        new System.Security.Claims.Claim(JwtClaimTypes.FamilyName, "Admin")
                    }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Console.WriteLine("admin created");
                }
                else
                {
                    Console.WriteLine("admin already exists");
                }
            }
        }
    }
}
