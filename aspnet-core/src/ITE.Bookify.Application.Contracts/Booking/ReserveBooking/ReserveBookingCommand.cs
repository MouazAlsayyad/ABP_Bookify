using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.ReserveBooking;

public sealed record ReserveBookingCommand(
    Guid ApartmentId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;
