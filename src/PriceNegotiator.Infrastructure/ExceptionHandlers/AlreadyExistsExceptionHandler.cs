using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceNegotiator.Application.Common.Exceptions.Base;

namespace PriceNegotiator.Infrastructure.ExceptionHandlers;

internal sealed class AlreadyExistsExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AlreadyExistsExceptionHandler> _logger;

    public AlreadyExistsExceptionHandler(ILogger<AlreadyExistsExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AlreadyExistsException alreadyExistsException)
        {
            return false;
        }

        _logger.LogError(
            alreadyExistsException,
            "AlreadyExistsException occurred: Value: {ExistsValue}",
            alreadyExistsException.ExistsValue);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Already Exist Error",
            Detail = $"'{alreadyExistsException.ExistsValue}' already exists."
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}