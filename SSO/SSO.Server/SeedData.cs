namespace SSO.Server
{
    using Domain.Entities;
    using Domain.EntityFramework;
    using IdentityModel;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AuthDbContext>();
                context.Database.Migrate();

                var emailClaim = context.Claims.FirstOrDefault((x) => x.Type == "email");

                if (emailClaim == null)
                    context.Claims.Add(new Claim { Type = "email" });

                var emailVerifiedClaim = context.Claims.FirstOrDefault((x) => x.Type == "email_verified");

                if (emailVerifiedClaim == null)
                    context.Claims.Add(new Claim { Type = "email_verified" });

                var familyNameClaim = context.Claims.FirstOrDefault((x) => x.Type == "family_name");

                if (familyNameClaim == null)
                    context.Claims.Add(new Claim { Type = "family_name" });

                var genderClaim = context.Claims.FirstOrDefault((x) => x.Type == "gender");

                if (genderClaim == null)
                    context.Claims.Add(new Claim { Type = "gender" });

                var givenNameClaim = context.Claims.FirstOrDefault((x) => x.Type == "given_name");

                if (givenNameClaim == null)
                    context.Claims.Add(new Claim { Type = "given_name" });

                var idpClaim = context.Claims.FirstOrDefault((x) => x.Type == "idp");

                if (idpClaim == null)
                    context.Claims.Add(new Claim { Type = "idp" });

                var middleNameClaim = context.Claims.FirstOrDefault((x) => x.Type == "middle_name");

                if (middleNameClaim == null)
                    context.Claims.Add(new Claim { Type = "middle_name" });

                var nameClaim = context.Claims.FirstOrDefault((x) => x.Type == "name");

                if (nameClaim == null)
                    context.Claims.Add(new Claim { Type = "name" });

                var preferredUsernameClaim = context.Claims.FirstOrDefault((x) => x.Type == "preferred_username");

                if (preferredUsernameClaim == null)
                    context.Claims.Add(new Claim { Type = "preferred_username" });

                context.SaveChanges();

                var emailIdentityResource = context.IdentityResources.FirstOrDefault((x) => x.Name == "email");

                if (emailIdentityResource == null)
                    context.IdentityResources.Add(new IdentityResource
                    {
                        Name = "email",
                        DisplayName = "Your email address",
                        Enabled = true,
                        Required = true,
                        Claims = new List<IdentityResourceClaim> {
                            new IdentityResourceClaim { ClaimId = emailClaim.Type },
                            new IdentityResourceClaim { ClaimId = emailVerifiedClaim.Type }
                        }
                    });

                var openidIdentityResource = context.IdentityResources.FirstOrDefault((x) => x.Name == "openid");

                if (openidIdentityResource == null)
                    context.IdentityResources.Add(new IdentityResource
                    {
                        Name = "Your user identifier",
                        Enabled = true,
                        Required = true,
                        Claims = new List<IdentityResourceClaim> {
                            new IdentityResourceClaim { ClaimId = idpClaim.Type }
                        }
                    });

                var profileIdentityResource = context.IdentityResources.FirstOrDefault((x) => x.Name == "profile");

                if (profileIdentityResource == null)
                    context.IdentityResources.Add(new IdentityResource
                    {
                        Name = "User profile",
                        Description = "Your user profile information (first name, last name, etc.)",
                        Enabled = true,
                        Required = true,
                        Claims = new List<IdentityResourceClaim> {
                            new IdentityResourceClaim{ ClaimId = familyNameClaim.Type  },
                            new IdentityResourceClaim{ ClaimId = givenNameClaim.Type  },
                            new IdentityResourceClaim{ ClaimId = middleNameClaim.Type  },
                            new IdentityResourceClaim{ ClaimId = nameClaim.Type  },
                            new IdentityResourceClaim{ ClaimId = preferredUsernameClaim.Type  }
                        }
                    });

                context.SaveChanges();

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
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin"
                    };

                    var result = userManager.CreateAsync(adminUser, "1071$1453#").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    var addRoleUser = userManager.AddToRoleAsync(adminUser, "auth.admin").Result;

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
