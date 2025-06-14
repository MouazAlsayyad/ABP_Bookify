using ITE.Bookify.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ITE.Bookify.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BookifyController : AbpControllerBase
{
    protected BookifyController()
    {
        LocalizationResource = typeof(BookifyResource);
    }
}
