using ITE.Bookify.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ITE.Bookify.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class BookifyDbContextFactory : IDesignTimeDbContextFactory<BookifyDbContext>
{
    public BookifyDbContext CreateDbContext(string[] args)
    {
        BookifyEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BookifyDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var dummyDateTimeProvider = new DateTimeProvider();

        return new BookifyDbContext(builder.Options, dummyDateTimeProvider);
    }
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ITE.Bookify.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
