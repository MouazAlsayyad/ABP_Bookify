using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingAlreadyStartedException : BusinessException
    {
        public BookingAlreadyStartedException()
            : base(BookifyDomainErrorCodes.BookingAlreadyStarted)
        {
        }
    }
}
