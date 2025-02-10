using Microsoft.Extensions.DependencyInjection;

namespace PriceNegotiator.Domain.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
    {
        services.AddMediatr();
        services.AddFluentValidation();

        return services;
    }
}