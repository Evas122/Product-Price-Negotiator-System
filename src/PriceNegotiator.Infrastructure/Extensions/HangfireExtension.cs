using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceNegotiator.Infrastructure.BackgroundJobs;

namespace PriceNegotiator.Infrastructure.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireSection = configuration.GetSection("Hangfire");

        services.AddHangfire(config =>
        {
            // in dev environment, in prduction better use local storage like sql or redis
            config.UseMemoryStorage();
        });
        services.AddHangfireServer();

        services.AddScoped<ExpiredNegotiationCancellationJob>();

        return services;
    }

    public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app, IConfiguration configuration)
    {

        var hangfireSection = configuration.GetSection("Hangfire");
        int negotiationStatusInterval = hangfireSection.GetValue<int>("NegotiationStatusInterval");

        var recurringJobId = hangfireSection.GetValue<string>("ExpiredNegotiationCancellationJobId");

        // * - minute, * - hour, * - day of month, * - month, * - day of week
        string cronExpression = $"*/{negotiationStatusInterval} * * * *";

        using var scope = app.ApplicationServices.CreateScope();
        var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

        RecurringJob.AddOrUpdate<ExpiredNegotiationCancellationJob>(
            recurringJobId: recurringJobId,
            methodCall: job => job.Execute(),
            cronExpression: cronExpression,
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc,   
            }
        );
        return app;
    }
}