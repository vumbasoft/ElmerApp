using Volo.Abp.Modularity;

namespace VumbaSoft.ErmanApp;

[DependsOn(
    typeof(ErmanAppApplicationModule),
    typeof(ErmanAppDomainTestModule)
)]
public class ErmanAppApplicationTestModule : AbpModule
{

}
