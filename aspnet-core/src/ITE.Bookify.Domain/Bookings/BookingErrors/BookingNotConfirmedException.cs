using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingNotConfirmedException : BusinessException
    {
        public BookingNotConfirmedException()
            : base(BookifyDomainErrorCodes.BookingNotConfirmed)
        {
        }
    }
}
