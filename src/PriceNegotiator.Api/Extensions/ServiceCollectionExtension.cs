namespace PriceNegotiator.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiExtensions(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocExtension();
        return services;
    }
}
