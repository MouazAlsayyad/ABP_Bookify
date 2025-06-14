using System.Threading.Tasks;

namespace ITE.Bookify.Data;

public interface IBookifyDbSchemaMigrator
{
    Task MigrateAsync();
}
