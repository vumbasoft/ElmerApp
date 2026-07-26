using VumbaSoft.ErmanApp.Localization;
using Volo.Abp.Application.Services;

namespace VumbaSoft.ErmanApp;

/* Inherit your application services from this class.
 */
public abstract class ErmanAppAppService : ApplicationService
{
    protected ErmanAppAppService()
    {
        LocalizationResource = typeof(ErmanAppResource);
    }
}
