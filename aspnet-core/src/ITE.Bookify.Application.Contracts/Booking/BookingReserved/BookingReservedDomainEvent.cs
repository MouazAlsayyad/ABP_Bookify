using ITE.Bookify.Abstractions;
using System;

namespace ITE.Bookify.Booking.BookingReserved;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;