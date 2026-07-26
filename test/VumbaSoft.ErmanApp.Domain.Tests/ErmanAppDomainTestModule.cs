using Volo.Abp.Modularity;

namespace VumbaSoft.ErmanApp;

[DependsOn(
    typeof(ErmanAppDomainModule),
    typeof(ErmanAppTestBaseModule)
)]
public class ErmanAppDomainTestModule : AbpModule
{

}
