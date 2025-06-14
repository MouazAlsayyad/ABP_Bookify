using ITE.Bookify.Booking;
using ITE.Bookify.Booking.CancelBooking;
using ITE.Bookify.Booking.CompleteBooking;
using ITE.Bookify.Booking.ConfirmBooking;
using ITE.Bookify.Booking.GetBooking;
using ITE.Bookify.Booking.RejectBooking;
using ITE.Bookify.Booking.ReserveBooking;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Bookings
{
    public class BookingService : ApplicationService, IBookingService
    {
        private readonly ISender _sender;

        public BookingService(ISender sender)
        {
            _sender = sender;
        }

        public async Task<BookingResponse> GetBooking(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);

            var result = await _sender.Send(query, cancellationToken);

            return result.Value;
        }

        public async Task<Guid> ReserveBooking(ReserveBookingDto request, CancellationToken cancellationToken)
        {
            var command = new ReserveBookingCommand(
                request.ApartmentId,
                request.StartDate,
                request.EndDate);

            var result = await _sender.Send(command, cancellationToken);
            return result.Value;
        }

        public async Task ConfirmBooking(Guid id, CancellationToken cancellationToken)
        {
            var command = new ConfirmBookingCommand(id);

            await _sender.Send(command, cancellationToken);
        }

        public async Task CancelBooking(Guid id, CancellationToken cancellationToken)
        {
            var command = new CancelBookingCommand(id);

            await _sender.Send(command, cancellationToken);
        }

        public async Task RejectBooking(Guid id, CancellationToken cancellationToken)
        {
            var command = new RejectBookingCommand(id);
            await _sender.Send(command, cancellationToken);
        }

        public async Task CompleteBooking(Guid id, CancellationToken cancellationToken)
        {
            var command = new CompleteBookingCommand(id);
            await _sender.Send(command, cancellationToken);
        }
    }
}
