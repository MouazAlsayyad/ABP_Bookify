using ITE.Bookify.Localization;
using Volo.Abp.Application.Services;

namespace ITE.Bookify;

/* Inherit your application services from this class.
 */
public abstract class BookifyAppService : ApplicationService
{
    protected BookifyAppService()
    {
        LocalizationResource = typeof(BookifyResource);
    }
}
