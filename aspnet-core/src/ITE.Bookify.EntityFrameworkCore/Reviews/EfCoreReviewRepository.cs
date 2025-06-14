using ITE.Bookify.EntityFrameworkCore;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ITE.Bookify.Reviews
{
    public class EfCoreReviewRepository : EfCoreRepository<BookifyDbContext, Review, Guid>,
        IReviewRepository,
        ITransientDependency
    {
        public EfCoreReviewRepository(IDbContextProvider<BookifyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
