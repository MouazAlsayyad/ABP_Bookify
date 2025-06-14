using ITE.Bookify.Abstractions;
using ITE.Bookify.Bookings.BookingErrors;
using ITE.Bookify.Users.UserErrors;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace ITE.Bookify.Bookings.Managers;
public class BookingValidationManager : IBookingValidationManager
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IIdentityUserRepository _userRepository;
    public BookingValidationManager(
        IBookingRepository bookingRepository,
        ICurrentUser currentUser,
        IIdentityUserRepository userRepository)
    {
        _bookingRepository = bookingRepository;
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task<Result<(Booking Booking, IdentityUser User)>> ValidateAllAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var bookingResult = await ValidateBookingAsync(bookingId, cancellationToken);
        if (bookingResult.IsFailure)
            return Result.Failure<(Booking, IdentityUser)>(bookingResult.Error);

        var userResult = await ValidateUserAsync();

        var booking = bookingResult.Value;
        var user = userResult.Value;

        ValidateBookingOwnership(booking, user);

        return Result.Success((booking, user));
    }

    public async Task<Result<Booking>> ValidateBookingAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetAsync(bookingId, cancellationToken: cancellationToken);
        return booking is null ? throw new BookingNotFoundException(bookingId) : Result.Success(booking);
    }

    public async Task<Result<IdentityUser>> ValidateUserAsync()
    {
        if (!_currentUser.Id.HasValue)
            throw new UserNotFoundException();

        var userId = _currentUser.Id.Value;

        var user = await _userRepository.GetAsync(userId) ?? throw new UserNotFoundException();

        return Result.Success(user);
    }

    public Result ValidateBookingOwnership(Booking booking, IdentityUser user)
    {
        return booking.UserId == user.Id
            ? Result.Success()
            : throw new UserNotAuthorizedException();
    }
}
