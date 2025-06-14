using System.Collections.Generic;

namespace ITE.Bookify.Apartments.CreateApartment;
public sealed record ApartmentCreateUpdateDto(
    string Name,
    string Description,
    AddressCreateUpdateDto Address,
    decimal PriceAmount,
    decimal CleaningFeeAmount,
    string Currency, // Currency code (e.g., "USD", "EUR")
    List<Amenity> Amenities
);

public sealed record AddressCreateUpdateDto(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street
);
