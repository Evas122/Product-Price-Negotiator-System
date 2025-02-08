using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Infrastructure.Data;
using PriceNegotiator.Infrastructure.Interfaces;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class DatabaseMigratorExtension
{
    public static void AddDatabaseMigrator(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseMigrator, DatabaseMigrator>();
    }
}