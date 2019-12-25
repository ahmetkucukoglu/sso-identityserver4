namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using Application.Infrastructure.IdentityServer;
    using Application.Infrastructure.IdentityServer.Ldap;
    using Application.User.Services.Ldap;
    using Domain.Entities;
    using Domain.EntityFramework;
    using IdentityModel;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class IdentityServerBuilderExtensions
    {
        public static IServiceCollection AddIdentityServer4(this IServiceCollection builder)
        {
            var serviceProvider = builder.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            builder.AddScoped<IIdentityUserService, LdapUserService>();
            builder.AddSingleton(new LdapUserStore(configuration.GetSection("Ldap").Get<LdapConfiguration>()));

            //builder.AddScoped<IIdentityUserService, IdentityUserService>();
            
            builder.AddAuthorization((options) =>
            {
                options.AddPolicy("Admin", (policy) =>
                {
                    policy.RequireRole("Medyanet - IT"); //Medyanet - IT //auth.admin
                });
            });

            builder.AddIdentity<ApplicationUser, IdentityRole>((options) =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            builder.Configure<IdentityOptions>((options) =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            builder.AddIdentityServer((options) =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.IssuerUri = configuration.GetValue<string>("IdentityServer:Url");
            })
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<LdapProfileService>()
            .AddDeveloperSigningCredential();

            return builder;
        }
    }
}
