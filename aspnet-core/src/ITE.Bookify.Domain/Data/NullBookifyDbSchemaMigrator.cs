using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ITE.Bookify.Data;

/* This is used if database provider does't define
 * IBookifyDbSchemaMigrator implementation.
 */
public class NullBookifyDbSchemaMigrator : IBookifyDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
