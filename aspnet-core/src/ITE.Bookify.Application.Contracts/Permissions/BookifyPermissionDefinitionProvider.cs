using ITE.Bookify.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ITE.Bookify.Permissions;

public class BookifyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BookifyPermissions.GroupName);

        var apartmentsPermission = myGroup.AddPermission(BookifyPermissions.Apartments.Default, L("Permission:Apartments"));
        apartmentsPermission.AddChild(BookifyPermissions.Apartments.Create, L("Permission:Apartments.Create"));
        apartmentsPermission.AddChild(BookifyPermissions.Apartments.Edit, L("Permission:Apartments.Edit"));

        var bookingsPermission = myGroup.AddPermission(BookifyPermissions.Bookings.Default, L("Permission:Bookings"));

        var reviewsPermission = myGroup.AddPermission(BookifyPermissions.Reviews.Default, L("Permission:Reviews"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookifyResource>(name);
    }
}
