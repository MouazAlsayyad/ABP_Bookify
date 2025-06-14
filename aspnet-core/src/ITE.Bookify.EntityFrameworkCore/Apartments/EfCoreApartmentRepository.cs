using ITE.Bookify.EntityFrameworkCore;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ITE.Bookify.Apartments
{
    public class EfCoreApartmentRepository
        : EfCoreRepository<BookifyDbContext, Apartments.Apartment, Guid>,
            IApartmentRepository,
            ITransientDependency
    {
        public EfCoreApartmentRepository(IDbContextProvider<BookifyDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
