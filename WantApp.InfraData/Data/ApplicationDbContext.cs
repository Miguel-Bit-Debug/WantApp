using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WantApp.Domain.Models.Product;

namespace WantApp.InfraData.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<Notification>();

        builder.Entity<Product>()
            .Property(x => x.Name).IsRequired();

        builder.Entity<Product>()
            .Property(x => x.Description).HasMaxLength(255);

        builder.Entity<Category>()
            .Property(x => x.Name).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
