﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PriceNegotiator.Application.Common.Interfaces;
using PriceNegotiator.Application.Common.Options;
using PriceNegotiator.Infrastructure.Repositories;
using PriceNegotiator.Infrastructure.Services;
using System.Text;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class AuthExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwtAuthentication(configuration);

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    private static void AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtService, JwtService>();
    }
}