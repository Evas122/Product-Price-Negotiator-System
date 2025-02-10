using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Infrastructure.DomainEvents;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class DomainEventsExtension
{
    public static void AddDomainEvents(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
    }
}