using System;
using Volo.Abp;

namespace ITE.Bookify.Bookings.BookingErrors
{
    public class BookingNotFoundException : BusinessException
    {
        public BookingNotFoundException(Guid Id)
            : base(BookifyDomainErrorCodes.BookingNotFound)
        {
            WithData("Id", Id);
        }
    }
}
