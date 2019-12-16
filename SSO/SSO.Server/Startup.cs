namespace SSO.Server
{
    using Application.ApiResource.Commands.CreateApiResource;
    using Application.ApiResource.Queries.GetApiResourceList;
    using Application.Infrastructure.AspNet;
    using Application.Infrastructure.IdentityServer;
    using Application.Infrastructure.IdentityServer.Extensions;
    using Application.Infrastructure.MediatR;
    using Domain.Entities;
    using Domain.EntityFramework;
    using FluentValidation.AspNetCore;
    using IdentityServer4.Stores;
    using Infrastructure.Email;
    using Infrastructure.Storage;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Linq;
    using System.Reflection;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AwsStorageSettings>(Configuration.GetSection("AwsStorage"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddScoped<IPasswordHasher<ApplicationUser>, ApplicationUserPasswordHasher>();
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IStorageService, AwsStorage>();
            services.AddScoped<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddMediatR(typeof(GetApiResourceListQuery).GetTypeInfo().Assembly);

            services.AddDbContext<AuthDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString(nameof(AuthDbContext)));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>((options) =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<ForwardedHeadersOptions>((options) =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();

                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
            });

            services.AddIdentityServer4();

            services.AddCors((options) =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    (builder) =>
                    {
                        var allowedOriginsSection = Configuration.GetSection("AllowedOrigins");
                        var allowedOrigins = allowedOriginsSection
                            .AsEnumerable()
                            .Select((x) => x.Value)
                            .Where((x) => !string.IsNullOrEmpty(x))
                            .ToArray();

                        builder.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
                    });
            })
            .AddControllersWithViews((options) =>
            {
                options.Filters.Add(typeof(FriendlyExceptionHandlingActionFilter));
            })
            .AddFluentValidation((options) =>
            {
                options.RegisterValidatorsFromAssemblyContaining<CreateApiResourceCommandValidator>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseHsts();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
