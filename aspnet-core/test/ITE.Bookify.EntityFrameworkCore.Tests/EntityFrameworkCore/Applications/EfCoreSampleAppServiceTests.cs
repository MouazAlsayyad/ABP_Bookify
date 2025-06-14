using ITE.Bookify.Samples;
using Xunit;

namespace ITE.Bookify.EntityFrameworkCore.Applications;

[Collection(BookifyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<BookifyEntityFrameworkCoreTestModule>
{

}
