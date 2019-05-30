namespace SSO.Application.Infrastructure.AspNet
{
    using Domain.EntityFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AuthDbContext>();
            var connectionString = configuration.GetConnectionString(nameof(AuthDbContext));

            builder.UseSqlServer(connectionString);

            return new AuthDbContext(builder.Options);
        }
    }
}
