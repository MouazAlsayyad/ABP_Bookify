using System;

namespace ITE.Bookify.Apartments.SearchApartments;

public sealed record SearchApartmentDto(
    DateOnly StartDate,
    DateOnly EndDate,
    int Page,
    int PageSize,
    string? SearchKey);
