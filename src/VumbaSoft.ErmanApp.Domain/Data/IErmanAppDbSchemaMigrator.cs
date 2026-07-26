using System.Threading.Tasks;

namespace VumbaSoft.ErmanApp.Data;

public interface IErmanAppDbSchemaMigrator
{
    Task MigrateAsync();
}
