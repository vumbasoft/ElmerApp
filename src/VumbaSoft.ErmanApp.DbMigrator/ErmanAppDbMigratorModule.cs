using VumbaSoft.ErmanApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace VumbaSoft.ErmanApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ErmanAppEntityFrameworkCoreModule),
    typeof(ErmanAppApplicationContractsModule)
)]
public class ErmanAppDbMigratorModule : AbpModule
{
}
