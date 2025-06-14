using ITE.Bookify.Messaging;
using ITE.Bookify.Shared;
using System;
using System.Collections.Generic;

namespace ITE.Bookify.Apartments.UpdateApartment;
public sealed record UpdateApartmentCommand(
    Guid Id,
    Name Name,
    Description Description,
    Address Address,
    Money Price,
    Money CleaningFee,
    List<Amenity> Amenities
) : ICommand<Guid>;
