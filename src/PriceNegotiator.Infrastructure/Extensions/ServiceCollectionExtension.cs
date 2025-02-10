using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddAuth(configuration);
        services.AddDateTimeProvider();
        services.AddRepositories();
        services.AddNegotiations();
        services.AddExceptionHandlers();
        services.AddDatabaseMigrator();
        services.AddHangfireJobs(configuration);

        return services;
    }
}