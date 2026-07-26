using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using VumbaSoft.ErmanApp.Localization;

namespace VumbaSoft.ErmanApp.Web;

[Dependency(ReplaceServices = true)]
public class ErmanAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ErmanAppResource> _localizer;

    public ErmanAppBrandingProvider(IStringLocalizer<ErmanAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
