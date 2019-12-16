namespace SSO.Server
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Sinks.Elasticsearch;
    using System;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            if (args.Contains("seed"))
            {
                SeedData.EnsureSeedData(webHost.Services);
            }

            webHost.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((webHostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(webHostBuilderContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(webHostBuilderContext.Configuration.GetValue<string>("Elastic:Uri"))));
                })
                .ConfigureWebHostDefaults((webBuilder) =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
