using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Infrastructure.Services;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class NegotiationExtension
{
    public static void AddNegotiations(this IServiceCollection services)
    {
        services.AddScoped<INegotiationValidationService, NegotiationValidationService>();
    }
}