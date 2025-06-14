namespace ITE.Bookify;

public static class BookifyDomainErrorCodes
{
    public const string ValidationErrors = "Booking:00000";

    public const string BookingNotFound = "Booking:00001";
    public const string BookingOverlap = "Booking:00002";
    public const string BookingNotReserved = "Booking:00003";
    public const string BookingNotConfirmed = "Booking:00004";
    public const string BookingAlreadyStarted = "Booking:00005";
    public const string BookingNotAuthorized = "Booking:00006";

    public const string UserNotFound = "Booking:00007";
    public const string UserNotAuthorized = "Booking:00008";

    public const string ApartmentConcurrency = "Booking:00009";
    public const string ApartmentInvalid = "Booking:00009";

    public const string RatingInvalid = "Booking:00010";
    public const string ReviewsNotEligible = "Booking:00011";
}
