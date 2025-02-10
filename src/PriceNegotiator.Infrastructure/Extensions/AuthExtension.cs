using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PriceNegotiator.Domain.Common.Constants;
using PriceNegotiator.Domain.Common.Options;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Infrastructure.Services;
using System.Text;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class AuthExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddJwtAuthentication(configuration);
        services.AddAuthorizationPolicies();

        services.AddAuthServices();
    }

    private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    private static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.EmployeePolicy, policy =>
            policy.RequireRole(UserRole.Employee.ToString()));
        });
    }

    private static void AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}