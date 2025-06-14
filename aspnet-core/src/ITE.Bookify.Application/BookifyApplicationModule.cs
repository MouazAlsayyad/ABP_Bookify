using ITE.Bookify.Bookings.Managers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FluentValidation;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

//[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ITE.Bookify;

[DependsOn(
    typeof(BookifyDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(BookifyApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpFluentValidationModule)
    )]
public class BookifyApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IBookingValidationManager, BookingValidationManager>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<BookifyApplicationModule>(validate: true);
        });

        context.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(BookifyApplicationModule).Assembly);

            //config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //config.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });

    }
}
