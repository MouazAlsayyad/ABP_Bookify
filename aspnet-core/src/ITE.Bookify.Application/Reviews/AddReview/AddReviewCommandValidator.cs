using FluentValidation;

namespace ITE.Bookify.Reviews.AddReview
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            RuleFor(command => command.BookingId)
                .NotEmpty()
                .WithMessage("Bookings ID is required.");

            RuleFor(command => command.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(command => command.Comment)
                .NotEmpty()
                .WithMessage("Comment is required.")
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}
