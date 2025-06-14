using Microsoft.Extensions.Localization;
using ITE.Bookify.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ITE.Bookify;

[Dependency(ReplaceServices = true)]
public class BookifyBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<BookifyResource> _localizer;

    public BookifyBrandingProvider(IStringLocalizer<BookifyResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
