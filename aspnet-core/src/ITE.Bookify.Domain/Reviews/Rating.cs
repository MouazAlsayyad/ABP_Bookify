using ITE.Bookify.Abstractions;
using ITE.Bookify.Reviews.ReviewsErrors;

namespace ITE.Bookify.Reviews;
public sealed record Rating
{

    private Rating(int value) => Value = value;

    public int Value { get; init; }

    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5)
            throw new RatingInvalidException();

        return new Rating(value);
    }
}
