namespace PriceNegotiator.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiExtensions(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDoc();
        return services;
    }
}