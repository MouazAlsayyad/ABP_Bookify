using Dapper;
using ITE.Bookify.Abstractions;
using ITE.Bookify.Booking.GetBooking;
using ITE.Bookify.Bookings.BookingErrors;
using ITE.Bookify.Data;
using ITE.Bookify.Messaging;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace ITE.Bookify.Bookings.GetBooking
{
    internal sealed class GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory, ICurrentUser currentUser) : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            const string sql = """
               SELECT
                   "Id" AS Id,
                   "ApartmentId" AS ApartmentId,
                   "UserId" AS UserId,
                   "Status" AS Status,
                   "PriceForPeriod_Amount" AS PriceAmount,
                   "PriceForPeriod_Currency" AS PriceCurrency,
                   "CleaningFee_Amount" AS CleaningFeeAmount,
                   "CleaningFee_Currency" AS CleaningFeeCurrency,
                   "AmenitiesUpCharge_Amount" AS AmenitiesUpChargeAmount,
                   "AmenitiesUpCharge_Currency" AS AmenitiesUpChargeCurrency,
                   "TotalPrice_Amount" AS TotalPriceAmount,
                   "TotalPrice_Currency" AS TotalPriceCurrency,
                   "Duration_Start" AS DurationStart,
                   "Duration_End" AS DurationEnd,
                   "CreatedOnUtc" AS CreatedOnUtc
               FROM "AppBookings" -- Corrected and quoted table name
               WHERE "Id" = @BookingId -- Quoted column name
               """;

            BookingResponse? booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(
            sql,
            new
            {
                request.BookingId
            });

            if (booking is null || booking.UserId != _currentUser.Id)
                throw new BookingNotFoundException(request.BookingId);

            return booking;
        }
    }
}
