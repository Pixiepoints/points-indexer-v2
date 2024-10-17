using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using PixiePointsApp.Entities;
using Points.Contracts.Point;

namespace PixiePointsApp.Processors;

public class JoinedLogEventProcessor : PixiePointsProcessorBase<Joined>
{
    public override async Task ProcessAsync(Joined eventValue, LogEventContext context)
    {
        // Logger.LogDebug("JoinedEvent: {eventValue} context: {context}", JsonConvert.SerializeObject(eventValue),
        //     JsonConvert.SerializeObject(context));

        var id = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Registrant.ToBase58());

        var user = await GetEntityAsync<OperatorUserIndex>(id);
        if (user != null)
        {
            Logger.LogWarning("User {User} of {DApp} exists", eventValue.Registrant.ToBase58(),
                eventValue.DappId.ToHex());
            return;
        }

        user = new OperatorUserIndex
        {
            Id = id,
            Domain = eventValue.Domain,
            Address = eventValue.Registrant.ToBase58(),
            DappName = eventValue.DappId.ToHex(),
            CreateTime = context.Block.BlockTime.ToUtcMilliSeconds()
        };

        await SaveEntityAsync(user, context);
    }
}