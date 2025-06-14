using ITE.Bookify.Booking.GetBooking;
using ITE.Bookify.Booking.ReserveBooking;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Booking
{
    public interface IBookingService : IApplicationService
    {
        Task<BookingResponse> GetBooking(Guid id, CancellationToken cancellationToken);
        Task<Guid> ReserveBooking(ReserveBookingDto request, CancellationToken cancellationToken);
        Task ConfirmBooking(Guid id, CancellationToken cancellationToken);
        Task CancelBooking(Guid id, CancellationToken cancellationToken);
        Task RejectBooking(Guid id, CancellationToken cancellationToken);
        Task CompleteBooking(Guid id, CancellationToken cancellationToken);
    }
}
