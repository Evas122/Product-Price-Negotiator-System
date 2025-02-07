namespace PriceNegotiator.Domain.Enums;

public enum NegotiationStatus
{
    WaitingForEmployee,
    Rejected,
    Accepted,
    Cancelled,
    AttemptsExceeded
}