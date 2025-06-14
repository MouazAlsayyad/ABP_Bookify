using ITE.Bookify.Abstractions;
using ITE.Bookify.Booking.ConfirmBooking;
using ITE.Bookify.Bookings.Managers;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Bookings.ConfirmBooking
{
    internal sealed class ConfirmBookingCommandHandler(IDateTimeProvider dateTimeProvider, IBookingValidationManager validationManager) : ICommandHandler<ConfirmBookingCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IBookingValidationManager _validationManager = validationManager;

        public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validationManager.ValidateAllAsync(request.BookingId, cancellationToken);

            (Booking booking, _) = validationResult.Value;

            booking.Confirm(_dateTimeProvider.UtcNow);

            return Result.Success();
        }
    }
}
