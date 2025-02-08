using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Infrastructure.ExceptionHandlers;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class ExceptionsHandlersExtension
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<InvalidCredentialsExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<AlreadyExistsExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        return services;
    }
}