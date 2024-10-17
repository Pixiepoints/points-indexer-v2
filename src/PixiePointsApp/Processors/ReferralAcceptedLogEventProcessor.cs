using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf;
using Newtonsoft.Json;
using PixiePointsApp.Entities;
using Points.Contracts.Point;

namespace PixiePointsApp.Processors;

public class ReferralAcceptedLogEventProcessor : PixiePointsProcessorBase<ReferralAccepted>
{
    public override async Task ProcessAsync(ReferralAccepted eventValue, LogEventContext context)
    {
        // Logger.LogInformation("ReferralAcceptedEvent: {eventValue} context: {context}",
        //     JsonConvert.SerializeObject(eventValue),
        //     JsonConvert.SerializeObject(context));

        var rawRecordId = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Referrer.ToBase58(),
            eventValue.Invitee.ToBase58());
        var recordId = HashHelper.ComputeFrom(rawRecordId).ToHex();

        var recordIndex = await GetEntityAsync<UserReferralRecordIndex>(recordId);
        if (recordIndex != null)
        {
            Logger.LogInformation("invite record already exist: {index}", JsonConvert.SerializeObject(recordIndex));
            return;
        }

        recordIndex = new UserReferralRecordIndex
        {
            Id = recordId,
            Domain = eventValue.Domain,
            DappId = eventValue.DappId.ToHex(),
            Referrer = eventValue.Referrer?.ToBase58() ?? string.Empty,
            Invitee = eventValue.Invitee?.ToBase58() ?? string.Empty,
            Inviter = eventValue.Inviter?.ToBase58() ?? string.Empty,
            CreateTime = context.Block.BlockTime.ToUtcMilliSeconds()
        };

        await SaveEntityAsync(recordIndex, context);

        var rawCountId = IdGenerateHelper.GetId(eventValue.DappId.ToHex(), eventValue.Referrer.ToBase58());
        var countId = HashHelper.ComputeFrom(rawCountId).ToHex();
        var countIndex = await GetEntityAsync<UserReferralCountIndex>(countId) ?? new UserReferralCountIndex
        {
            Id = countId,
            Domain = eventValue.Domain,
            DappId = eventValue.DappId.ToHex(),
            Referrer = eventValue.Referrer?.ToBase58() ?? string.Empty,
            InviteeNumber = 0,
            CreateTime = context.Block.BlockTime.ToUtcMilliSeconds(),
            UpdateTime = context.Block.BlockTime.ToUtcMilliSeconds()
        };
        countIndex.InviteeNumber += 1;

        await SaveEntityAsync(countIndex, context);

        // Logger.LogInformation("ReferralAcceptedEventProcessFinished");
    }
}