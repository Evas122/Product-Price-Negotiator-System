using MediatR;

namespace PriceNegotiator.Domain.Interfaces.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>;