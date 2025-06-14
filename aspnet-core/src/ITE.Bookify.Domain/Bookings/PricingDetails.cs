using ITE.Bookify.Shared;

namespace ITE.Bookify.Bookings;
public sealed record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice);

