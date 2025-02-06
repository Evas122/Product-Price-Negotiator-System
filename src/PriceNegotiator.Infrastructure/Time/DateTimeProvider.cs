using PriceNegotiator.Application.Interfaces;

namespace PriceNegotiator.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}