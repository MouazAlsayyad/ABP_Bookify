using Dapper;
using ITE.Bookify.Abstractions;
using ITE.Bookify.Apartments.ApartmentErrors;
using ITE.Bookify.Data;
using ITE.Bookify.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Apartments.GetApartment
{
    internal sealed class GetApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetApartmentQuery, ApartmentResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<Result<ApartmentResponse>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    a."Id" AS Id,
                    a."Name" AS Name,
                    a."Description" AS Description,
                    a."Price_Amount" AS Price,
                    a."Price_Currency" AS Currency,
                    (SELECT AVG(CAST(r_agg."Rating" AS DECIMAL(10,2))) FROM "AppReviews" AS r_agg WHERE r_agg."ApartmentId" = a."Id") AS AverageRating,
                    (SELECT COUNT(r_agg."Id") FROM "AppReviews" AS r_agg WHERE r_agg."ApartmentId" = a."Id") AS ReviewCount,

                    a."Address_Country" AS Country,
                    a."Address_State" AS State,
                    a."Address_ZipCode" AS ZipCode,
                    a."Address_City" AS City,
                    a."Address_Street" AS Street,

                    a."Amenities" AS AmenitiesArray,

                    r."Id" AS ReviewId,
                    r."UserId" AS UserId,
                    u."UserName" AS FirstName, -- Mapped to ReviewResponse.FirstName
                    r."Rating" AS Rating,
                    r."Comment" AS Comment,
                    r."CreationTime" AS CreatedOnUtc
                FROM "AppApartments" AS a
                LEFT JOIN "AppReviews" AS r ON a."Id" = r."ApartmentId"
                LEFT JOIN "AbpUsers" AS u ON r."UserId" = u."Id"
                WHERE a."Id" = @ApartmentId
            """;

            ApartmentResponse? apartmentResult = null;
            var reviewsForApartment = new List<ReviewResponse>();

            await connection.QueryAsync<ApartmentResponse, AddressResponse, int[], ReviewResponse, ApartmentResponse>(
                sql,
                (apartment, address, amenitiesArray, review) =>
                {
                    if (apartmentResult == null)
                    {
                        apartmentResult = apartment; // Now apartment.AverageRating and apartment.ReviewCount will be correctly populated by Dapper
                        apartmentResult.Address = address;
                        apartmentResult.Amenities = amenitiesArray?.Select(aItem => (Amenity)aItem).ToList() ?? []; // Use a different variable name to avoid confusion with outer scope 'amenities' if any
                        apartmentResult.Reviews = reviewsForApartment;
                    }

                    // Ensure review is not null and has a valid ID before adding.
                    // This check is important because of the LEFT JOIN; if an apartment has no reviews,
                    // 'review' will be an object with all default values (Guid.Empty for ReviewId).
                    if (review != null && review.ReviewId != Guid.Empty)
                    {
                        // Ensure not to add duplicate reviews if an apartment has multiple amenities
                        // but only one review, or other complex join scenarios.
                        // However, with the current SQL, one row per review is expected.
                        if (!reviewsForApartment.Any(r => r.ReviewId == review.ReviewId))
                        {
                            reviewsForApartment.Add(review);
                        }
                    }

                    // Dapper doesn't use this return value directly to build a list when the lambda itself manages the state (like apartmentResult).
                    // Returning apartmentResult or null is fine.
                    return apartmentResult; // Or return null;
                },
                new { request.ApartmentId },
                splitOn: "Country,AmenitiesArray,ReviewId"); // Correct splitOn matching the new column order

            if (apartmentResult == null)
            {
                // Throwing a custom exception or returning a failure Result is better than re-throwing a generic one.
                // return Result.Failure<ApartmentResponse>(ApartmentErrors.NotFound); // Example if you have a typed error system
                throw new ApartmentNotFoundException(typeof(Apartments.Apartment), request.ApartmentId);
            }

            // If apartmentResult is not null but has no reviews, AverageRating might be null (if AVG returns NULL)
            // and ReviewCount will be 0. This is correct.
            // If it has reviews, these should now be populated correctly.

            return Result.Success(apartmentResult);
        }
    }
}
