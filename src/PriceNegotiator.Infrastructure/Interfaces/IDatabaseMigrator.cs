namespace PriceNegotiator.Infrastructure.Interfaces;

public interface IDatabaseMigrator
{
    Task EnsureMigrationAsync();
}