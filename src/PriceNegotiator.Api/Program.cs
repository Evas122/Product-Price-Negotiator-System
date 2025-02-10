using PriceNegotiator.Api.Extensions;
using PriceNegotiator.Domain.Extensions;
using PriceNegotiator.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureExtensions(builder.Configuration);
builder.Services.AddApiExtensions();
builder.Services.AddApplicationExtensions();

builder.Services.AddRouting(options =>
options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireJobs(builder.Configuration);

await app.EnsureDatabaseMigratedAsync();

app.Run();