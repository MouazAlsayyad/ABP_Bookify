using ITE.Bookify.Abstractions;
using ITE.Bookify.Apartments;
using ITE.Bookify.Apartments.ApartmentErrors;
using ITE.Bookify.Booking.ReserveBooking;
using ITE.Bookify.Bookings.BookingErrors;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using ITE.Bookify.Users.UserErrors;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace ITE.Bookify.Bookings.BookingReserved
{
    internal sealed class ReserveBookingCommandHandler(IApartmentRepository apartmentRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IPricingManager pricingManager, IDateTimeProvider dateTimeProvider, ICurrentUser currentUser) : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IApartmentRepository _apartmentRepository = apartmentRepository;
        private readonly IBookingRepository _bookingRepository = bookingRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPricingManager _pricingManager = pricingManager;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Id == null)
                throw new UserNotFoundException();

            var apartment = await _apartmentRepository.GetAsync(request.ApartmentId, cancellationToken: cancellationToken)
                ?? throw new ApartmentNotFoundException(typeof(Apartments.Apartment), request.ApartmentId);
            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
                throw new BookingOverlapException();

            try
            {
                var booking = Booking.Reserve(
                    apartment,
                    (Guid)_currentUser.Id,
                    duration,
                    _dateTimeProvider.UtcNow,
                    _pricingManager
                );

                await _bookingRepository.InsertAsync(booking, cancellationToken: cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return booking.Id;
            }
            catch (Exception)
            {
                throw new BookingOverlapException();
            }
        }
    }
}
