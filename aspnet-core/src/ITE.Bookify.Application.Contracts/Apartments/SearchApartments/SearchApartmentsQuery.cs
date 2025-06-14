using ITE.Bookify.Messaging;
using System;
using System.Collections.Generic;

namespace ITE.Bookify.Apartments.SearchApartments;

public sealed record SearchApartmentsQuery(
    DateOnly StartDate,
    DateOnly EndDate,
    int Page,
    int PageSize,
    string? SearchKey) : ICachedQuery<IReadOnlyList<SearchApartmentResponse>>
{
    public string CacheKey => $"apartments-{StartDate}-{EndDate}-{Page}-{PageSize}-{SearchKey}";

    public TimeSpan? Expiration => null;
}
