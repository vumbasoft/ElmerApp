using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VumbaSoft.ErmanApp.Data;
using Volo.Abp.DependencyInjection;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore;

public class EntityFrameworkCoreErmanAppDbSchemaMigrator
    : IErmanAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreErmanAppDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ErmanAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ErmanAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
