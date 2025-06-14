using ITE.Bookify.Abstractions;
using MediatR;

namespace ITE.Bookify.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

