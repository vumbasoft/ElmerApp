using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace VumbaSoft.ErmanApp.Data;

/* This is used if database provider does't define
 * IErmanAppDbSchemaMigrator implementation.
 */
public class NullErmanAppDbSchemaMigrator : IErmanAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
