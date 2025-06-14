using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Booking.CompleteBooking;
public sealed record CompleteBookingCommand(Guid BookingId) : ICommand;