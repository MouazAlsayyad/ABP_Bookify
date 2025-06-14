using ITE.Bookify.Bookings.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;

namespace ITE.Bookify.Bookings.BookingReserved
{
    internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        public BookingReservedDomainEventHandler(IBookingRepository bookingRepository, IIdentityUserRepository userRepository, IEmailSender emailSender)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetAsync(notification.BookingId, cancellationToken: cancellationToken);

            if (booking is null)
            {
                return;
            }

            var user = await _userRepository.GetAsync(booking.UserId, cancellationToken: cancellationToken);

            if (user is null)
            {
                return;
            }

            await _emailSender.SendAsync(
                user.Email,
                "Bookings reserved!",         // subject
                "You have 10 minutes to confirm this booking"  // email body
            );
        }
    }
}
