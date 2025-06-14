using ITE.Bookify.Messaging;
using System;

namespace ITE.Bookify.Reviews.AddReview;

public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand<Guid>;