using System;
using System.Collections.Generic;

namespace ITE.Bookify.Apartments;

public sealed class SearchApartmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }

    public AddressResponse Address { get; set; }
    public List<Amenity> Amenities { get; set; }

    public SearchApartmentResponse()
    {
        Amenities = [];
    }
}
