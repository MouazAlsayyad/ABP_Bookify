using ITE.Bookify.Reviews.AddReview;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Reviews;
public interface IReviewService : IApplicationService
{
    Task<Guid> AddRevie(AddReviewDto input, CancellationToken cancellationToken);
}

