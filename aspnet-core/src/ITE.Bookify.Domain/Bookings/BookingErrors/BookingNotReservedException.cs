using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingNotReservedException : BusinessException
    {
        public BookingNotReservedException()
            : base(BookifyDomainErrorCodes.BookingNotReserved)
        {

        }
    }
}
