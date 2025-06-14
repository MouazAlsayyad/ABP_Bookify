using System;
using Volo.Abp.Domain.Repositories;

namespace ITE.Bookify.Reviews
{
    public interface IReviewRepository : IRepository<Review, Guid>
    {
    }
}
