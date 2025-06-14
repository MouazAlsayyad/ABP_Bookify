using Volo.Abp.Modularity;

namespace ITE.Bookify;

[DependsOn(
    typeof(BookifyDomainModule),
    typeof(BookifyTestBaseModule)
)]
public class BookifyDomainTestModule : AbpModule
{

}
