using ITE.Bookify.Abstractions;
using ITE.Bookify.Bookings;
using ITE.Bookify.Bookings.Managers;
using ITE.Bookify.Clock;
using ITE.Bookify.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace ITE.Bookify.Reviews.AddReview
{
    internal sealed class AddReviewCommandHandler : ICommandHandler<AddReviewCommand, Guid>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBookingValidationManager _validationManager;

        public AddReviewCommandHandler(IBookingRepository bookingRepository, IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IBookingValidationManager validationManager)
        {
            _bookingRepository = bookingRepository;
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _validationManager = validationManager;
        }

        public async Task<Result<Guid>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validationManager.ValidateAllAsync(request.BookingId, cancellationToken);

            (var booking, _) = validationResult.Value;


            var ratingResult = Rating.Create(request.Rating);

            var reviewResult = Review.Create(
                booking,
                ratingResult.Value,
                new Comment(request.Comment),
                _dateTimeProvider.UtcNow);

            var review = await _reviewRepository.InsertAsync(reviewResult.Value, cancellationToken: cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}
