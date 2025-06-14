using ITE.Bookify.Abstractions;
using ITE.Bookify.Booking.CompleteBooking;
using ITE.Bookify.Bookings.Managers;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace ITE.Bookify.Bookings.CompleteBooking
{
    internal sealed class CompleteBookingCommandHandler(IDateTimeProvider dateTimeProvider, IBookingValidationManager validationService) : ICommandHandler<CompleteBookingCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IBookingValidationManager _validationService = validationService;

        public async Task<Result> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            Result<(Booking Booking, IdentityUser User)> validationResult =
                 await _validationService.ValidateAllAsync(request.BookingId, cancellationToken);

            (Booking booking, _) = validationResult.Value;

            booking.Complete(_dateTimeProvider.UtcNow);

            return Result.Success();
        }
    }
}
