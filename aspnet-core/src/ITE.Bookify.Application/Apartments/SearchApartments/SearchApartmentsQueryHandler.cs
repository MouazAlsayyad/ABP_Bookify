using Dapper;
using ITE.Bookify.Abstractions;
using ITE.Bookify.Bookings;
using ITE.Bookify.Data;
using ITE.Bookify.Messaging;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Apartments.SearchApartments
{
    internal sealed class SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<SearchApartmentResponse>>
    {
        private static readonly int[] ActiveBookingStatuses =
        [
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed
        ];

        public async Task<Result<IReadOnlyList<SearchApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.CreateConnection();

            // SQL query with conditional SearchKey filtering
            const string sql = """
                SELECT
                    a."Id" AS Id,
                    a."Name" AS Name,
                    a."Description" AS Description,
                    a."Price_Amount" AS Price,
                    a."Price_Currency" AS Currency,
                    COALESCE(AVG(CAST(r."Rating" AS DECIMAL(10,2))), 0.0) AS AverageRating, 
                    COUNT(DISTINCT r."Id") AS ReviewCount, 

                    a."Address_Country" AS Country,
                    a."Address_State" AS State,
                    a."Address_ZipCode" AS ZipCode,
                    a."Address_City" AS City,
                    a."Address_Street" AS Street,
                    a."Amenities" AS AmenitiesArray
                FROM "AppApartments" AS a
                LEFT JOIN "AppReviews" AS r ON a."Id" = r."ApartmentId"
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM "AppBookings" AS b
                    WHERE
                        b."ApartmentId" = a."Id" AND
                        b."Duration_Start" <= @EndDateParam AND
                        b."Duration_End" >= @StartDateParam AND
                        b."Status" = ANY(@ActiveBookingStatusesParam)
                )
                -- Add SearchKey filter conditionally
                AND (@SearchKeyParam IS NULL OR
                    a."Name" ILIKE @SearchPattern OR
                    a."Description" ILIKE @SearchPattern OR
                    a."Address_Country" ILIKE @SearchPattern OR
                    a."Address_State" ILIKE @SearchPattern OR
                    a."Address_City" ILIKE @SearchPattern OR
                    a."Address_Street" ILIKE @SearchPattern OR
                    a."Address_ZipCode" ILIKE @SearchPattern)
                GROUP BY
                    a."Id",
                    a."Name",
                    a."Description",
                    a."Price_Amount",
                    a."Price_Currency",
                    a."Address_Country",
                    a."Address_State",
                    a."Address_ZipCode",
                    a."Address_City",
                    a."Address_Street",
                    a."Amenities"
                ORDER BY a."Id"
                OFFSET @OffsetParam ROWS
                FETCH NEXT @PageSizeParam ROWS ONLY
                """;

            var offset = (request.Page - 1) * request.PageSize;

            // Prepare parameters, including SearchKey and SearchPattern
            var searchKey = request.SearchKey;
            var searchPattern = !string.IsNullOrWhiteSpace(searchKey) ? $"%{searchKey.Trim()}%" : null;

            var parameters = new
            {
                StartDateParam = request.StartDate,
                EndDateParam = request.EndDate,
                ActiveBookingStatusesParam = ActiveBookingStatuses,
                OffsetParam = offset,
                PageSizeParam = request.PageSize,
                SearchKeyParam = searchKey, // Used for the IS NULL check in SQL
                SearchPattern = searchPattern // Used for ILIKE comparisons in SQL
            };

            var apartmentEntries = await connection.QueryAsync<SearchApartmentResponse, AddressResponse, int[], SearchApartmentResponse>(
               sql,
               (apartment, address, amenitiesIntArray) =>
               {
                   apartment.Address = address;
                   apartment.Amenities = amenitiesIntArray?.Select(amenityValue => (Amenity)amenityValue).ToList()
                                         ?? [];
                   return apartment;
               },
               parameters,
               splitOn: "Country,AmenitiesArray");

            return Result.Success<IReadOnlyList<SearchApartmentResponse>>(apartmentEntries.ToList());
        }
    }
}
