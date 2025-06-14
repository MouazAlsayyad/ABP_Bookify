using ITE.Bookify.Apartments;
using ITE.Bookify.Bookings;
using ITE.Bookify.Clock;
using ITE.Bookify.Outbox;
using ITE.Bookify.Reviews;
using ITE.Bookify.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ITE.Bookify.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class BookifyDbContext :
    AbpDbContext<BookifyDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    #region Entities from the modules
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<Volo.Abp.Identity.OrganizationUnit> OrganizationUnits { get; set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly IDateTimeProvider _dateTimeProvider;

    public BookifyDbContext(DbContextOptions<BookifyDbContext> options, IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(BookifyDbContext).Assembly);
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Review>(b =>
        {
            b.ToTable(BookifyConsts.DbTablePrefix + "Reviews", BookifyConsts.DbSchema);
            b.HasKey(review => review.Id);
            b.Property(review => review.Rating)
                .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

            b.Property(review => review.Comment)
                .HasMaxLength(200)
                .HasConversion(comment => comment.Value, value => new Comment(value));

            b.HasOne<Apartment>()
               .WithMany()
               .HasForeignKey(review => review.ApartmentId);

            b.HasOne<Booking>()
               .WithMany()
               .HasForeignKey(review => review.BookingId);

            b.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(review => review.UserId);

            b.ConfigureByConvention();
        });

        builder.Entity<Apartment>(b =>
        {
            b.ToTable(BookifyConsts.DbTablePrefix + "Apartments", BookifyConsts.DbSchema);

            b.HasKey(apartment => apartment.Id);

            b.OwnsOne(apartment => apartment.Address);

            b.Property(apartment => apartment.Name)
                .HasMaxLength(200)
                .HasConversion(name => name.Value, value => new Name(value));

            b.Property(apartment => apartment.Description)
                .HasMaxLength(2000)
                .HasConversion(description => description.Value, value => new Description(value));

            b.OwnsOne(apartment => apartment.Price, priceBuilder =>
            priceBuilder.Property(money => money.Currency)
                        .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.OwnsOne(apartment => apartment.CleaningFee, priceBuilder =>
                priceBuilder.Property(money => money.Currency)
                            .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.Property<uint>("Version").IsRowVersion();
            b.ConfigureByConvention();
        });

        builder.Entity<Booking>(b =>
        {
            b.ToTable(BookifyConsts.DbTablePrefix + "Bookings", BookifyConsts.DbSchema);

            b.HasKey(apartment => apartment.Id);

            b.OwnsOne(booking => booking.PriceForPeriod, priceBuilder => priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.OwnsOne(booking => booking.CleaningFee, priceBuilder => priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.OwnsOne(booking => booking.AmenitiesUpCharge, priceBuilder => priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.OwnsOne(booking => booking.TotalPrice, priceBuilder => priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.FromCode(code)));

            b.OwnsOne(booking => booking.Duration);

            b.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(booking => booking.ApartmentId);

            b.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(booking => booking.UserId);

            b.ConfigureByConvention();
        });


        builder.Entity<OutboxMessage>(b =>
        {
            b.ToTable(BookifyConsts.DbTablePrefix + "OutboxMessages", BookifyConsts.DbSchema);

            b.HasKey(outboxMessage => outboxMessage.Id);
            b.Property(outboxMessage => outboxMessage.Content).HasColumnType("jsonb");
            b.ConfigureByConvention();
        });
    }

}
