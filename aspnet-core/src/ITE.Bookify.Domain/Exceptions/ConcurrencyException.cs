using Volo.Abp;

namespace ITE.Bookify.Exceptions;

public sealed class ConcurrencyException(string message) : BusinessException(message)
{
}
