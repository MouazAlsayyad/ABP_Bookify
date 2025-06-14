using Volo.Abp;

namespace ITE.Bookify.Reviews.ReviewsErrors;
public class RatingInvalidException : BusinessException
{
    public RatingInvalidException() : base(BookifyDomainErrorCodes.RatingInvalid)
    {

    }
}
