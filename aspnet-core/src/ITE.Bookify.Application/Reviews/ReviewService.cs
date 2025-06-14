using ITE.Bookify.Reviews.AddReview;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Reviews
{
    public class ReviewService : ApplicationService, IReviewService
    {
        private readonly ISender _sender;
        public ReviewService(ISender sender)
        {
            _sender = sender;
        }
        public async Task<Guid> AddRevie(AddReviewDto input, CancellationToken cancellationToken)
        {
            var command = new AddReviewCommand(input.BookingId, input.Rating, input.Comment);

            var result = await _sender.Send(command, cancellationToken);

            return result.Value;
        }
    }
}
