using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Application.Common.Behaviors;
using System.Reflection;

namespace PriceNegotiator.Application.Extensions;

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