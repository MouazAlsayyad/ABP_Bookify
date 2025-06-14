using ITE.Bookify.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ITE.Bookify.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BookifyEntityFrameworkCoreModule),
    typeof(BookifyApplicationContractsModule)
    )]
public class BookifyDbMigratorModule : AbpModule
{
}
