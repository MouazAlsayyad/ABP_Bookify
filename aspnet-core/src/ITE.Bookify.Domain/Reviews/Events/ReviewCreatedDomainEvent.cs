using ITE.Bookify.Abstractions;
using System;

namespace ITE.Bookify.Reviews.Events;
public sealed record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;
