using ITE.Bookify.Abstractions;
using System;

namespace ITE.Bookify.Bookings.Events;
public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;
