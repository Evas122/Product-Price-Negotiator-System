using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Api.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerDocExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Price Negotiator API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            c.EnableAnnotations();
            c.UseInlineDefinitionsForEnums();

            c.MapType<EmployeeAction>(() => new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetNames(typeof(EmployeeAction))
                               .Select(name => new OpenApiString(name))
                               .ToList<IOpenApiAny>()
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
        return services;
    }
}