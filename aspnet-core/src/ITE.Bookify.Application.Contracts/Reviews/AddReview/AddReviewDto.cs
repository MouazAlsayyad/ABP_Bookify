using System;

namespace ITE.Bookify.Reviews.AddReview;
public sealed record AddReviewDto(Guid BookingId, int Rating, string Comment);
