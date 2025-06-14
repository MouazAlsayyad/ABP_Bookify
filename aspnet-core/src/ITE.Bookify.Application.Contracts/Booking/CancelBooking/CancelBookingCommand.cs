using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.CancelBooking;
public sealed record CancelBookingCommand(Guid BookingId) : ICommand;
