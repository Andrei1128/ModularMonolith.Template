using Core.Common.Models;
using MediatR;

namespace Core.Contracts.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;