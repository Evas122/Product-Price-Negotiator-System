using PriceNegotiator.Application.Common.Interfaces;

namespace PriceNegotiator.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}