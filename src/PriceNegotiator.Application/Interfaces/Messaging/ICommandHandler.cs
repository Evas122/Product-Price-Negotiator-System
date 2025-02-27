﻿using MediatR;

namespace PriceNegotiator.Application.Interfaces.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit> where TCommand : ICommand;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>;