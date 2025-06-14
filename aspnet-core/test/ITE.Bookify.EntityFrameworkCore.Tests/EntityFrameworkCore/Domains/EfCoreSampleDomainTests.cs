using ITE.Bookify.Samples;
using Xunit;

namespace ITE.Bookify.EntityFrameworkCore.Domains;

[Collection(BookifyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BookifyEntityFrameworkCoreTestModule>
{

}
