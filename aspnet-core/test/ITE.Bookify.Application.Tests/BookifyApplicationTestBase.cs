using Volo.Abp.Modularity;

namespace ITE.Bookify;

public abstract class BookifyApplicationTestBase<TStartupModule> : BookifyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
