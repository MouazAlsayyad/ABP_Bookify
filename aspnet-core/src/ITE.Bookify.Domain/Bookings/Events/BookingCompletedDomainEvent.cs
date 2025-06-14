using ITE.Bookify.Abstractions;
using System;

namespace ITE.Bookify.Bookings.Events;
public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;
