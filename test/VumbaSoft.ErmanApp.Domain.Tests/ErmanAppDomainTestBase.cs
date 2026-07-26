using Volo.Abp.Modularity;

namespace VumbaSoft.ErmanApp;

/* Inherit from this class for your domain layer tests. */
public abstract class ErmanAppDomainTestBase<TStartupModule> : ErmanAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
