using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceNegotiator.Domain.Common.Exceptions.Base;

namespace PriceNegotiator.Infrastructure.ExceptionHandlers;

internal sealed class InvalidCredentialsExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidCredentialsExceptionHandler> _logger;

    public InvalidCredentialsExceptionHandler(ILogger<InvalidCredentialsExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidCredentialsException credentialsException)
        {
            return false;
        }

        _logger.LogWarning(credentialsException, "Invalid credentials error occurred: {Message}", credentialsException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Invalid Credentials",
            Detail = credentialsException.Message
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}