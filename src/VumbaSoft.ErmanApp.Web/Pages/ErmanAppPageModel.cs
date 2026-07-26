using VumbaSoft.ErmanApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace VumbaSoft.ErmanApp.Web.Pages;

public abstract class ErmanAppPageModel : AbpPageModel
{
    protected ErmanAppPageModel()
    {
        LocalizationResourceType = typeof(ErmanAppResource);
    }
}
