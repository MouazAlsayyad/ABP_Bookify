using System;
using Volo.Abp.Domain.Repositories;

namespace ITE.Bookify.Apartments
{
    public interface IApartmentRepository : IRepository<Apartment, Guid>
    {
    }
}
