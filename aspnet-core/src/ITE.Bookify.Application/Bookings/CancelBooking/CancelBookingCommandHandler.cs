using ITE.Bookify.Abstractions;
using ITE.Bookify.Booking.CancelBooking;
using ITE.Bookify.Bookings.Managers;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Bookings.CancelBooking
{
    internal sealed class CancelBookingCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IBookingValidationManager validationService) : ICommandHandler<CancelBookingCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IBookingValidationManager _validationService = validationService;

        public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validationService.ValidateAllAsync(request.BookingId, cancellationToken);

            (Booking booking, _) = validationResult.Value;

            booking.Cancel(_dateTimeProvider.UtcNow);

            return Result.Success();
        }
    }
}
