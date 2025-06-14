using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Apartments.GetApartment;
public sealed record GetApartmentQuery(Guid ApartmentId) : ICachedQuery<ApartmentResponse>
{
    public string CacheKey => $"apartments-{ApartmentId}";

    public TimeSpan? Expiration => null;
}