namespace SSO.Domain.EntityFramework
{
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Grant> Grants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Grant>().HasKey((x) => x.Key);
            builder.Entity<IdentityResource>().HasKey((x) => x.Name);
            builder.Entity<ApiResource>().HasKey((x) => x.Name);
            builder.Entity<Claim>().HasKey((x) => x.Type);
            builder.Entity<DataProtectionKey>().HasKey((x) => x.FriendlyName);

            builder.Entity<ClientIdentityResource>().HasKey((x) => new { x.ClientId, x.IdentityResourceId });
            builder.Entity<ClientApiResource>().HasKey((x) => new { x.ClientId, x.ApiResourceId });
            builder.Entity<IdentityResourceClaim>().HasKey((x) => new { x.IdentityResourceId, x.ClaimId });
            builder.Entity<ApiResourceClaim>().HasKey((x) => new { x.ApiResourceId, x.ClaimId });

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
