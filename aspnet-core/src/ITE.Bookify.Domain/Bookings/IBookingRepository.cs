using ITE.Bookify.Apartments;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ITE.Bookify.Bookings
{
    public interface IBookingRepository : IRepository<Booking, Guid>
    {
        Task<bool> IsOverlappingAsync(
        Apartment apartment,
        DateRange duration,
        CancellationToken cancellationToken = default);
    }
}
