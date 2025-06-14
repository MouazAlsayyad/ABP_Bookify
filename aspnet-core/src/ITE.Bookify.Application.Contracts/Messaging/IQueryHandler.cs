using ITE.Bookify.Abstractions;
using MediatR;

namespace ITE.Bookify.Messaging;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}

