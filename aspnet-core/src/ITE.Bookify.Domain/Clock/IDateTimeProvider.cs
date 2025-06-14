using System;

namespace ITE.Bookify.Clock;
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
