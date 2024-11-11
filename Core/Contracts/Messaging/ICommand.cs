using Core.Common.Models;
using MediatR;

namespace Core.Contracts.Messaging;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;