using Core.Common.Models;
using MediatR;

namespace Core.Contracts.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;