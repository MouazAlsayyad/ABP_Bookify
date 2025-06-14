using System;

namespace ITE.Bookify.Booking.ReserveBooking;
public sealed record ReserveBookingDto(
        Guid ApartmentId,
        DateOnly StartDate,
        DateOnly EndDate);
