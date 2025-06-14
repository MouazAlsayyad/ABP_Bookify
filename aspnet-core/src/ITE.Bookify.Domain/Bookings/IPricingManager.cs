using ITE.Bookify.Apartments;

namespace ITE.Bookify.Bookings;
public interface IPricingManager
{
    PricingDetails CalculatePrice(Apartment apartment, DateRange period);
}
