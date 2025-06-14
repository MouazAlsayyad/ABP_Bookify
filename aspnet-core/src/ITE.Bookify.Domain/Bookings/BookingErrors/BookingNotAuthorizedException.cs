using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingNotAuthorizedException : BusinessException
    {
        public BookingNotAuthorizedException(string name)
            : base(BookifyDomainErrorCodes.BookingNotAuthorized)
        {
            WithData("name", name);
        }
    }
}
