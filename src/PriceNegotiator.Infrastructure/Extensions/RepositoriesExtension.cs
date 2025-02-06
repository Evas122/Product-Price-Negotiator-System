using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Services;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class RepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}