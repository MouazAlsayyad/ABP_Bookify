using System;
using Volo.Abp.Domain.Entities;

namespace ITE.Bookify.Apartments.ApartmentErrors;
public class ApartmentNotFoundException : EntityNotFoundException
{
    public ApartmentNotFoundException(Type entityType, object? id)
            : base(entityType, id)
    {
    }
}
