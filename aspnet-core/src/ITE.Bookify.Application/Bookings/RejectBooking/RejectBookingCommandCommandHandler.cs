using ITE.Bookify.Abstractions;
using ITE.Bookify.Booking.RejectBooking;
using ITE.Bookify.Bookings.Managers;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Bookings.RejectBooking
{
    internal sealed class RejectBookingCommandCommandHandler(IDateTimeProvider dateTimeProvider, IBookingValidationManager validationManager) : ICommandHandler<RejectBookingCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IBookingValidationManager _validationManager = validationManager;

        public async Task<Result> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validationManager.ValidateAllAsync(request.BookingId, cancellationToken);

            (Booking booking, _) = validationResult.Value;

            booking.Reject(_dateTimeProvider.UtcNow);

            return Result.Success();
        }
    }
}
