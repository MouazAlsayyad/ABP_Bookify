namespace ITE.Bookify.Permissions;

public static class BookifyPermissions
{
    public const string GroupName = "Bookify";

    public static class Apartments
    {
        public const string Default = GroupName + ".Apartments";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
    }

    public static class Bookings
    {
        public const string Default = GroupName + ".Bookings";
    }

    public static class Reviews
    {
        public const string Default = GroupName + ".Reviews";
    }
}
