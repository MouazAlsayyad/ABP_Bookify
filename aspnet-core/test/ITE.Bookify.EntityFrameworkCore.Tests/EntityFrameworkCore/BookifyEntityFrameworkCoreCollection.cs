using Xunit;

namespace ITE.Bookify.EntityFrameworkCore;

[CollectionDefinition(BookifyTestConsts.CollectionDefinitionName)]
public class BookifyEntityFrameworkCoreCollection : ICollectionFixture<BookifyEntityFrameworkCoreFixture>
{

}
