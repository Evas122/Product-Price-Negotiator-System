using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Domain.Common.Behaviors;
using System.Reflection;

namespace PriceNegotiator.Domain.Extensions;

public static class MediatrExtension
{
    public static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }
}