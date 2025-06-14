using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.RejectBooking;

public sealed record RejectBookingCommand(Guid BookingId) : ICommand;
