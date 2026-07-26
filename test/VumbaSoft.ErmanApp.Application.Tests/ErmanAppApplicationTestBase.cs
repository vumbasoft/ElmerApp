using Volo.Abp.Modularity;

namespace VumbaSoft.ErmanApp;

public abstract class ErmanAppApplicationTestBase<TStartupModule> : ErmanAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
