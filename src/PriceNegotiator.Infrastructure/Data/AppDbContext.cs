using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Negotiation> Negotiations { get; set; }
    public DbSet<NegotiationAttempt> NegotiationAttempts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}