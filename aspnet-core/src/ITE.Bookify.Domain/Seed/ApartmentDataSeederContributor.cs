using Bogus;
using Dapper;
using ITE.Bookify.Apartments;
using ITE.Bookify.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace ITE.Bookify.Seed;
internal class ApartmentDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApartmentDataSeederContributor> _logger;

    public ApartmentDataSeederContributor(
        ISqlConnectionFactory sqlConnectionFactory,
        IConfiguration configuration,
        ILogger<ApartmentDataSeederContributor> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (!Convert.ToBoolean(_configuration["DataSeeding:EnableApartmentSeed"] ?? "false"))
        {
            _logger.LogInformation("Apartment data seeding is disabled in configuration.");
            return;
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        if (connection.State != ConnectionState.Open)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open database connection for seeding.");
                return; // Or throw, depending on desired behavior
            }
        }


        var count = 0;
        try
        {
            count = await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM public.""AppApartments""");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, @"Could not check count of AppApartments. This might be okay if migrations haven't run yet. Error: {ErrorMessage}", ex.Message);
        }

        if (count > 0)
        {
            _logger.LogInformation("AppApartments table already has {Count} records. Skipping seed.", count);
            return;
        }

        _logger.LogInformation("Seeding AppApartments table...");

        var faker = new Faker();

        var apartments = new List<object>();
        for (var i = 0; i < 20; i++) // Reduced for faster seeding during dev
        {
            apartments.Add(new
            {
                Id = Guid.NewGuid(), // Or _guidGenerator.Create() if using ABP services
                Name = faker.Company.CompanyName().ClampLength(200),
                Description = faker.Lorem.Paragraph(1).ClampLength(2000),
                Address_Country = faker.Address.Country(),
                Address_State = faker.Address.State(),
                Address_ZipCode = faker.Address.ZipCode(),
                Address_City = faker.Address.City(),
                Address_Street = faker.Address.StreetAddress(),
                Price_Amount = faker.Random.Decimal(50, 1000),
                Price_Currency = "USD",
                CleaningFee_Amount = faker.Random.Decimal(25, 200),
                CleaningFee_Currency = "USD",
                LastBookedOnUtc = i % 4 == 0 ? (DateTime?)null : DateTime.UtcNow.AddDays(-faker.Random.Int(1, 90)),
                Amenities = new[] { (int)faker.PickRandom<Amenity>(), (int)faker.PickRandom<Amenity>() }.Distinct().ToArray(),
                ExtraProperties = "{}",
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                CreationTime = DateTime.UtcNow,
                IsDeleted = false
            });
        }

        const string sql = """
            INSERT INTO public."AppApartments"
            ("Id", "Name", "Description",
             "Address_Country", "Address_State", "Address_ZipCode", "Address_City", "Address_Street",
             "Price_Amount", "Price_Currency", "CleaningFee_Amount", "CleaningFee_Currency",
             "LastBookedOnUtc", "Amenities",
             "ExtraProperties", "ConcurrencyStamp", "CreationTime", "IsDeleted"
             )
            VALUES(
             @Id, @Name, @Description,
             @Address_Country, @Address_State, @Address_ZipCode, @Address_City, @Address_Street,
             @Price_Amount, @Price_Currency, @CleaningFee_Amount, @CleaningFee_Currency,
             @LastBookedOnUtc, @Amenities,
             @ExtraProperties, @ConcurrencyStamp, @CreationTime, @IsDeleted
            );
            """;

        try
        {
            var insertedCount = await connection.ExecuteAsync(sql, apartments);
            _logger.LogInformation("Successfully seeded {InsertedCount} apartments into AppApartments.", insertedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding AppApartments data.");
            // Consider if you need to throw the exception to halt the seeding process
            // or if logging the error is sufficient.
        }
    }

}
