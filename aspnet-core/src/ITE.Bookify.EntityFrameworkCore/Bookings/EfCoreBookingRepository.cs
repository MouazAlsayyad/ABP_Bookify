using ITE.Bookify.Apartments;
using ITE.Bookify.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ITE.Bookify.Bookings
{
    public class EfCoreBookingRepository : EfCoreRepository<BookifyDbContext, Booking, Guid>,
        IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
        [
            BookingStatus.Reserved,
            BookingStatus.Confirmed,
            BookingStatus.Completed
        ];
        public EfCoreBookingRepository(IDbContextProvider<BookifyDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
             .AnyAsync
            (
                booking =>
                    booking.ApartmentId == apartment.Id &&
                    booking.Duration.Start <= duration.End &&
                    booking.Duration.End >= duration.Start &&
                    ActiveBookingStatuses.Contains(booking.Status),
                cancellationToken
            );

        }
    }
}
