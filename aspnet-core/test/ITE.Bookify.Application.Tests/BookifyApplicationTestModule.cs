using Volo.Abp.Modularity;

namespace ITE.Bookify;

[DependsOn(
    typeof(BookifyApplicationModule),
    typeof(BookifyDomainTestModule)
)]
public class BookifyApplicationTestModule : AbpModule
{

}
