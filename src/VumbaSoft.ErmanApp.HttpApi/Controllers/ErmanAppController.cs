using VumbaSoft.ErmanApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace VumbaSoft.ErmanApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ErmanAppController : AbpControllerBase
{
    protected ErmanAppController()
    {
        LocalizationResource = typeof(ErmanAppResource);
    }
}
