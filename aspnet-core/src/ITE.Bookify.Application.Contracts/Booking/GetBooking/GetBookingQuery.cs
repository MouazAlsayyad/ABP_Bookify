using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.GetBooking;
public sealed record GetBookingQuery(Guid BookingId) : ICachedQuery<BookingResponse>
{
    public string CacheKey => $"bookings-{BookingId}";

    public TimeSpan? Expiration => null;
}
