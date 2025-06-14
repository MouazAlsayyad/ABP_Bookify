using ITE.Bookify.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace ITE.Bookify.Bookings.Managers;

public interface IBookingValidationManager
{
    Task<Result<(Booking Booking, IdentityUser User)>> ValidateAllAsync(Guid bookingId, CancellationToken cancellationToken);
    Task<Result<Booking>> ValidateBookingAsync(Guid bookingId, CancellationToken cancellationToken);
    Task<Result<IdentityUser>> ValidateUserAsync();
    Result ValidateBookingOwnership(Booking booking, IdentityUser user);
}
