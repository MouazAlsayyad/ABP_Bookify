using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingOverlapException : BusinessException
    {
        public BookingOverlapException()
            : base(BookifyDomainErrorCodes.BookingOverlap)
        {
        }
    }
}
