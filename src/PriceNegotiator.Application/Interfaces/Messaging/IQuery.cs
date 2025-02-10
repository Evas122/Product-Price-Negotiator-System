using MediatR;

namespace PriceNegotiator.Application.Interfaces.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>;