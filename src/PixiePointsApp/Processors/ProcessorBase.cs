using AeFinder.Sdk.Entities;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace PixiePointsApp.Processors;

public abstract class ProcessorBase<TEvent> : LogEventProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    
    protected async Task SaveEntityAsync<TEntity>(TEntity index, LogEventContext context) where TEntity : AeFinderEntity
    {
        ObjectMapper.Map(context, index);
        await SaveEntityAsync(index);
    }

    protected async Task DeleteEntityAsyncAndCheck<TEntity>(string id) where TEntity : AeFinderEntity
    {
        var index = await GetEntityAsync<TEntity>(id);
        if (index != null)
        {
            await DeleteEntityAsync<TEntity>(id);
        }
    }
}