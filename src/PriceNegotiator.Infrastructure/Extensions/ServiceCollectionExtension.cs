using Microsoft.Extensions.DependencyInjection;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services)
    {
        services.AddDateTimeProvider();
        services.AddRepositories();
        return services;
    }
}