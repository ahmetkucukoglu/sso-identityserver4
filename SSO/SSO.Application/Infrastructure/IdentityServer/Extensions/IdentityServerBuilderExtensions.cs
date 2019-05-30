namespace SSO.Application.Infrastructure.IdentityServer.Extensions
{
    using SSO.Domain.Entities;
    using IdentityModel;
    using IdentityServer4;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCert(this IIdentityServerBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();

            var hostingEnvironment = serviceProvider.GetService<IHostingEnvironment>();
            var configuration = serviceProvider.GetService<IConfiguration>();

            if (hostingEnvironment.IsProduction())
            { 
                builder.AddSigningCredential(configuration.GetValue<string>("IdentityServer:Thumbprint"), StoreLocation.LocalMachine, NameType.Thumbprint);

                builder.AddValidationKey(configuration.GetValue<string>("IdentityServer:Thumbprint"), StoreLocation.LocalMachine, NameType.Thumbprint);
            }

            return builder;
        }

        public static IIdentityServerBuilder AddAuth(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransientDecorator<IUserClaimsPrincipalFactory<ApplicationUser>, AuthUserClaimsPrincipalFactory>();

            builder.Services.Configure<IdentityOptions>((options) =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            builder.Services.Configure<SecurityStampValidatorOptions>((options) =>
            {
                options.OnRefreshingPrincipal = SecurityStampValidatorCallback.UpdatePrincipal;
            });

            builder.Services.ConfigureApplicationCookie((options) =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            builder.Services.ConfigureExternalCookie((options) =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorRememberMeScheme, (options) =>
            {
                options.Cookie.IsEssential = true;
            });

            builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorUserIdScheme, (options) =>
            {
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication((options) =>
            {
                if (options.DefaultAuthenticateScheme == null && options.DefaultScheme == IdentityServerConstants.DefaultCookieAuthenticationScheme)
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                }
            });

            builder.AddResourceOwnerValidator<AuthResourceOwnerPasswordValidator>();
            builder.AddProfileService<AuthProfileService>();

            return builder;
        }

        internal static void AddTransientDecorator<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddDecorator<TService>();
            services.AddTransient<TService, TImplementation>();
        }

        internal static void AddDecorator<TService>(this IServiceCollection services)
        {
            var registration = services.LastOrDefault(x => x.ServiceType == typeof(TService));

            if (registration == null)
            {
                throw new InvalidOperationException("Service type: " + typeof(TService).Name + " not registered.");
            }
            if (services.Any(x => x.ServiceType == typeof(AuthDecorator<TService>)))
            {
                throw new InvalidOperationException("Decorator already registered for type: " + typeof(TService).Name + ".");
            }

            services.Remove(registration);

            if (registration.ImplementationInstance != null)
            {
                var type = registration.ImplementationInstance.GetType();
                var innerType = typeof(AuthDecorator<,>).MakeGenericType(typeof(TService), type);

                services.Add(new ServiceDescriptor(typeof(AuthDecorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, registration.ImplementationInstance));
            }
            else if (registration.ImplementationFactory != null)
            {
                services.Add(new ServiceDescriptor(typeof(AuthDecorator<TService>), provider =>
                {
                    return new DisposableAuthDecorator<TService>((TService)registration.ImplementationFactory(provider));
                }, registration.Lifetime));
            }
            else
            {
                var type = registration.ImplementationType;
                var innerType = typeof(AuthDecorator<,>).MakeGenericType(typeof(TService), registration.ImplementationType);

                services.Add(new ServiceDescriptor(typeof(AuthDecorator<TService>), innerType, ServiceLifetime.Transient));
                services.Add(new ServiceDescriptor(type, type, registration.Lifetime));
            }
        }
    }
}
