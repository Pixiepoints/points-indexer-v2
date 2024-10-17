using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using PixiePointsApp.Processors;
using Volo.Abp.Modularity;

namespace PixiePointsApp;

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(PixiePointsAppModule))]
public class PixiePointsAppTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<PixiePointsAppModule>(); });

        // Add your Processors.
        context.Services.AddSingleton<AppliedLogEventProcessor>();
        context.Services.AddSingleton<PointsLogEventProcessor>();
        context.Services.AddSingleton<ReferralAcceptedLogEventProcessor>();
        context.Services.AddSingleton<JoinedLogEventProcessor>();
    }
}