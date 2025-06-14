using Volo.Abp;

namespace ITE.Bookify.Users.UserErrors;
public class UserNotAuthorizedException : BusinessException
{
    public UserNotAuthorizedException()
    : base(BookifyDomainErrorCodes.UserNotFound)
    {
    }
}