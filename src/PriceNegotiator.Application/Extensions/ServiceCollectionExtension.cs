using Microsoft.Extensions.DependencyInjection;

namespace PriceNegotiator.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
    {
        services.AddMediatrExtension();
        services.AddFluentValidation();

        return services;
    }
}