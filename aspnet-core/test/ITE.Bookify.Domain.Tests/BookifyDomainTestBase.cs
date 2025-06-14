using Volo.Abp.Modularity;

namespace ITE.Bookify;

/* Inherit from this class for your domain layer tests. */
public abstract class BookifyDomainTestBase<TStartupModule> : BookifyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
