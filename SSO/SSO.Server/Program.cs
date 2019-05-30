namespace SSO.Server
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
