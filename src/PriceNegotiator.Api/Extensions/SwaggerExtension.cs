using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Api.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Price Negotiator API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter 'Bearer' [space] and then your token in the text input below."
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

            c.MapType<ProblemDetails>(() => CreateProblemDetailsSchema());

            c.MapType<ValidationProblemDetails>(() => CreateValidationProblemDetailsSchema());
        });

        services.AddSwaggerGenNewtonsoftSupport();
        return services;
    }

    private static OpenApiSchema CreateProblemDetailsSchema()
    {
        return new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["title"] = new OpenApiSchema { Type = "string" },
                ["status"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["detail"] = new OpenApiSchema { Type = "string" }
            },
            Example = new OpenApiObject
            {
                ["title"] = new OpenApiString("Status title"),
                ["status"] = new OpenApiInteger(0),
                ["detail"] = new OpenApiString("details of occured problem")
            }
        };
    }
    private static OpenApiSchema CreateValidationProblemDetailsSchema()
    {
        return new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["title"] = new OpenApiSchema { Type = "string" },
                ["status"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                ["errors"] = new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema { Type = "string" }
                }
            },
            Example = new OpenApiObject
            {
                ["title"] = new OpenApiString("One or more validation errors occurred."),
                ["status"] = new OpenApiInteger(400),
                ["errors"] = new OpenApiObject
                {
                    ["prop1"] = new OpenApiArray
                    {
                        new OpenApiString("prop1 is required.")
                    }
                }
            }
        };
    }
}