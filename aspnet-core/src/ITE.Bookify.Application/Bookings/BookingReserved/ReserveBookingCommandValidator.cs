using FluentValidation;
using ITE.Bookify.Booking.ReserveBooking;

namespace ITE.Bookify.Bookings.BookingReserved
{
    internal class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(c => c.ApartmentId).NotEmpty();

            RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
        }
    }
}
