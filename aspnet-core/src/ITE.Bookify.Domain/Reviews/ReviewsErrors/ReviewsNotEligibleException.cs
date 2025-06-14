using Volo.Abp;

namespace ITE.Bookify.Reviews.ReviewsErrors;
public class ReviewsNotEligibleException : BusinessException
{
    public ReviewsNotEligibleException() : base(BookifyDomainErrorCodes.ReviewsNotEligible)
    {

    }
}
