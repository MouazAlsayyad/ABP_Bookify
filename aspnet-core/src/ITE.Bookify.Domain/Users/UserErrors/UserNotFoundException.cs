using Volo.Abp;

namespace ITE.Bookify.Users.UserErrors;
public class UserNotFoundException : BusinessException
{
    public UserNotFoundException()
    : base(BookifyDomainErrorCodes.UserNotFound)
    {
    }
}