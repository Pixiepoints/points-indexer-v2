using AeFinder.Sdk.Processor;
using PixiePointsApp.GraphQL;
using PixiePointsApp.Processors;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace PixiePointsApp;

public class PixiePointsAppModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<PixiePointsAppModule>(); });
        context.Services.AddSingleton<ISchema, PointsIndexerPluginSchema>();
        
        // Add your LogEventProcessor implementation.
        context.Services.AddSingleton<ILogEventProcessor, AppliedLogEventProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, JoinedLogEventProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, PointsLogEventProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, ReferralAcceptedLogEventProcessor>();
    }
}