using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Infrastructure.Time;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class DateTimeProviderExtension
{
    public static void AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}