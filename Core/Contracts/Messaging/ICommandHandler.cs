using Core.Common.Models;
using MediatR;

namespace Core.Contracts.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;