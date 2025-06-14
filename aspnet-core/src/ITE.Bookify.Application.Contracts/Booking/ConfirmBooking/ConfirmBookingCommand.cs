using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.ConfirmBooking;
public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;
