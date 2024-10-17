using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf;
using Newtonsoft.Json;
using PixiePointsApp.Entities;
using Points.Contracts.Point;

namespace PixiePointsApp.Processors;

public class AppliedLogEventProcessor : PixiePointsProcessorBase<InviterApplied>
{
    public override async Task ProcessAsync(InviterApplied eventValue, LogEventContext context)
    {
        // Logger.LogDebug("AppliedEvent: {eventValue} context: {context}", JsonConvert.SerializeObject(eventValue),
        //     JsonConvert.SerializeObject(context));

        var id = HashHelper.ComputeFrom(eventValue.Domain).ToHex();
        var domainIndex = await GetEntityAsync<OperatorDomainIndex>(id);

        if (domainIndex != null)
        {
            Logger.LogInformation("domain already exist: {index}", JsonConvert.SerializeObject(domainIndex));
            return;
        }

        domainIndex = new OperatorDomainIndex
        {
            Id = id,
            Domain = eventValue.Domain,
            DepositAddress = eventValue.Invitee.ToBase58(),
            DappId = eventValue.DappId.ToHex(),
            CreateTime = context.Block.BlockTime
        };

        if (eventValue.Inviter != null)
        {
            domainIndex.InviterAddress = eventValue.Inviter.ToBase58();
        }

        await SaveEntityAsync(domainIndex, context);
    }
}