using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Infrastructure.Interfaces;

namespace PriceNegotiator.Infrastructure.Data;

public class DatabaseMigrator : IDatabaseMigrator
{
    private readonly AppDbContext _dbContext;

    public DatabaseMigrator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task EnsureMigrationAsync()
    {
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}