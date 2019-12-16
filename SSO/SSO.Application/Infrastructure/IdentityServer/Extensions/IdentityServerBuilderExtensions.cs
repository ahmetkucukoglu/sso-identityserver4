namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using IdentityModel;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SSO.Domain.Entities;

    public static class IdentityServerBuilderExtensions
    {
        public static IServiceCollection AddIdentityServer4(this IServiceCollection builder)
        {
            var serviceProvider = builder.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

           
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
            .AddDeveloperSigningCredential();


            return builder;
        }
    }
}
