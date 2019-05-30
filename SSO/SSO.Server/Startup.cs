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
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.Elasticsearch;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            var elasticUri = Configuration["Elastic:Uri"];

            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Verbose()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
               {
                   MinimumLogEventLevel = LogEventLevel.Information,
                   AutoRegisterTemplate = true,
               })
               .CreateLogger();

            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GoogleCloudStorageSettings>(Configuration.GetSection("GoogleCloudStorage"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddScoped<IPasswordHasher<ApplicationUser>, ApplicationUserPasswordHasher>();
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IStorageService, GoogleCloudStorage>();
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

            services.Configure<IISOptions>((options) =>
            {
                options.AuthenticationDisplayName = "Windows";
                options.AutomaticAuthentication = false;
            });

            services.AddIdentityServer((options) =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.IssuerUri = Configuration.GetValue<string>("IdentityServer:Url");
            })
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddAuth()
            .AddCert();

            //services.AddDataProtection()
            //    .SetApplicationName("Auth")
            //    .PersistKeysToFileSystem(new DirectoryInfo(Configuration.GetValue<string>("DataProtection:Path")));

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

                        builder.WithOrigins(allowedOrigins).AllowAnyMethod();
                    });
            })
            .AddMvc((options) =>
            {
                options.Filters.Add(typeof(FriendlyExceptionHandlingActionFilter));
            })
            .AddFluentValidation((options) =>
            {
                options.RegisterValidatorsFromAssemblyContaining<CreateApiResourceCommandValidator>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddSerilog();

            app.UseCors("AllowSpecificOrigin")
                .UseHsts()
                .UseStaticFiles()
                .UseIdentityServer()
                .UseMvcWithDefaultRoute();
        }
    }
}
