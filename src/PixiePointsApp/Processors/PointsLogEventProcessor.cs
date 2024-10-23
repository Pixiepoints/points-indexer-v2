using System.Numerics;
using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf;
using PixiePointsApp.Entities;
using Points.Contracts.Point;

namespace PixiePointsApp.Processors;

public class PointsLogEventProcessor : PixiePointsProcessorBase<PointsChanged>
{
    public override async Task ProcessAsync(PointsChanged eventValue, LogEventContext context)
    {
        // Logger.LogDebug("PointsRecorded: {eventValue} context: {context}", JsonConvert.SerializeObject(eventValue),
        //     JsonConvert.SerializeObject(context));

        foreach (var pointsDetail in eventValue.PointsChangedDetails.PointsDetails)
        {
            var balanceValue = pointsDetail.BalanceValue?.Value ?? pointsDetail.Balance.ToString();
            var increaseValue = pointsDetail.IncreaseValue?.Value ?? pointsDetail.IncreaseAmount.ToString();

            var rawLogIndexId = IdGenerateHelper.GetId(context.Transaction.TransactionId, pointsDetail.DappId.ToHex(),
                pointsDetail.PointsReceiver.ToBase58(), pointsDetail.IncomeSourceType, pointsDetail.ActionName,
                pointsDetail.PointsName, balanceValue, increaseValue);
            var pointsLogIndexId = HashHelper.ComputeFrom(rawLogIndexId).ToHex();

            var pointsLogIndex = await GetEntityAsync<AddressPointsLogIndex>(pointsLogIndexId);
            if (pointsLogIndex != null)
            {
                Logger.LogInformation("Duplicated event index: {index}", pointsLogIndex);
                continue;
            }

            pointsLogIndex = ObjectMapper.Map<PointsChangedDetail, AddressPointsLogIndex>(pointsDetail);
            pointsLogIndex.Amount = pointsDetail.IncreaseValue?.Value ?? pointsDetail.IncreaseAmount.ToString();
            pointsLogIndex.Id = pointsLogIndexId;
            pointsLogIndex.CreateTime = context.Block.BlockTime;

            await SaveEntityAsync(pointsLogIndex, context);

            var rawActionIndexId = IdGenerateHelper.GetId(pointsDetail.DappId.ToHex(),
                pointsDetail.PointsReceiver.ToBase58(), pointsDetail.Domain, pointsDetail.ActionName,
                pointsDetail.IncomeSourceType);
            var pointsActionIndexId = HashHelper.ComputeFrom(rawActionIndexId).ToHex();

            var pointsActionIndex = await GetEntityAsync<AddressPointsSumByActionIndex>(pointsActionIndexId);
            if (pointsActionIndex != null)
            {
                var amount = ParseBigInteger(pointsActionIndex.Amount) + ParseBigInteger(increaseValue);
                pointsActionIndex.Amount = amount.ToString();
            }
            else
            {
                pointsActionIndex = ObjectMapper.Map<PointsChangedDetail, AddressPointsSumByActionIndex>(pointsDetail);
                pointsActionIndex.Id = pointsActionIndexId;
                pointsActionIndex.Amount = increaseValue;
                pointsActionIndex.CreateTime = context.Block.BlockTime;
            }

            ObjectMapper.Map(context, pointsActionIndex);
            pointsActionIndex.UpdateTime = context.Block.BlockTime;
            await SaveEntityAsync(pointsActionIndex);

            var rawSymbolIndexId = IdGenerateHelper.GetId(pointsDetail.DappId.ToHex(),
                pointsDetail.PointsReceiver.ToBase58(),
                pointsDetail.Domain, pointsDetail.IncomeSourceType);
            var pointsSymbolIndexId = HashHelper.ComputeFrom(rawSymbolIndexId).ToHex();

            var pointsIndex = await GetEntityAsync<AddressPointsSumBySymbolIndex>(pointsSymbolIndexId);
            if (pointsIndex != null)
            {
                if (pointsIndex.UpdateTime > context.Block.BlockTime)
                {
                    continue;
                }

                ObjectMapper.Map(context, pointsIndex);
                var needUpdated = UpdatePoint(pointsDetail, pointsIndex, out var newIndex);
                if (!needUpdated)
                {
                    continue;
                }

                newIndex.UpdateTime = context.Block.BlockTime;
                await SaveEntityAsync(newIndex);
            }
            else
            {
                pointsIndex = ObjectMapper.Map<PointsChangedDetail, AddressPointsSumBySymbolIndex>(pointsDetail);
                ObjectMapper.Map(context, pointsIndex);

                UpdatePoint(pointsDetail, pointsIndex, out var newIndex);

                newIndex.Id = pointsSymbolIndexId;
                newIndex.CreateTime = context.Block.BlockTime;
                newIndex.UpdateTime = context.Block.BlockTime;
                await SaveEntityAsync(newIndex);
            }
        }
    }

    private static bool UpdatePoint(PointsChangedDetail pointsState, AddressPointsSumBySymbolIndex originIndex,
        out AddressPointsSumBySymbolIndex newIndex)
    {
        newIndex = originIndex;
        var symbol = pointsState.PointsName;
        var amount = pointsState.BalanceValue?.Value ?? pointsState.Balance.ToString();
        if (symbol.EndsWith("-1"))
        {
            newIndex.FirstSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-2"))
        {
            newIndex.SecondSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-3"))
        {
            newIndex.ThirdSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-4"))
        {
            newIndex.FourSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-5"))
        {
            newIndex.FiveSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-6"))
        {
            newIndex.SixSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-7"))
        {
            newIndex.SevenSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-8"))
        {
            newIndex.EightSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-9"))
        {
            newIndex.NineSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-10"))
        {
            newIndex.TenSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-11"))
        {
            newIndex.ElevenSymbolAmount = amount;
        }
        else if (symbol.EndsWith("-12"))
        {
            newIndex.TwelveSymbolAmount = amount;
        }
        else
        {
            return false;
        }

        return true;
    }

    private static BigInteger ParseBigInteger(string input)
    {
        return BigInteger.TryParse(input, out var result) ? result : 0;
    }
}