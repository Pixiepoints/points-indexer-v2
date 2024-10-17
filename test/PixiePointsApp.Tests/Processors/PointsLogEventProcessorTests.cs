using AeFinder.Sdk;
using AElf;
using AElf.Types;
using PixiePointsApp.Entities;
using PixiePointsApp.GraphQL;
using PixiePointsApp.GraphQL.Dto;
using Points.Contracts.Point;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace PixiePointsApp.Processors;

public class PointsLogEventProcessorTests : PixiePointsAppTestBase
{
    private readonly PointsLogEventProcessor _pointsLogEventProcessor;
    private readonly IReadOnlyRepository<AddressPointsSumBySymbolIndex> _symbolIndexRepository;
    private readonly IReadOnlyRepository<AddressPointsSumByActionIndex> _actionIndexRepository;
    private readonly IReadOnlyRepository<AddressPointsLogIndex> _logIndexRepository;
    private readonly IObjectMapper _objectMapper;

    public PointsLogEventProcessorTests()
    {
        _pointsLogEventProcessor = GetRequiredService<PointsLogEventProcessor>();
        _symbolIndexRepository = GetRequiredService<IReadOnlyRepository<AddressPointsSumBySymbolIndex>>();
        _actionIndexRepository = GetRequiredService<IReadOnlyRepository<AddressPointsSumByActionIndex>>();
        _logIndexRepository = GetRequiredService<IReadOnlyRepository<AddressPointsLogIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task HandlePointsUpdatedProcessor()
    {
        var pointsDetailList = new PointsChangedDetails();
        pointsDetailList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            Balance = 10000000,
            DappId = Hash.LoadFromHex("abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6")
        });
        pointsDetailList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            Balance = 20000000,
            DappId = Hash.LoadFromHex("abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6")
        });
        pointsDetailList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            Balance = 40000000,
            DappId = Hash.LoadFromHex("abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6")
        });

        var pointsUpdated = new PointsChanged
        {
            PointsChangedDetails = pointsDetailList
        };

        var logEventContext = GenerateLogEventContext(pointsUpdated);
        await _pointsLogEventProcessor.ProcessAsync(logEventContext);

        var pointsSumBySymbol = await Query.GetPointsSumBySymbol(_symbolIndexRepository, _objectMapper,
            new GetPointsSumBySymbolDto
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                MaxResultCount = 10
            });
        pointsSumBySymbol.TotalRecordCount.ShouldBe(0);
    }


    [Fact]
    public async Task HandlePointsRecordedProcessor()
    {
        var pointsRecordList = new PointsChangedDetails();
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            IncreaseAmount = 100000,
            IncreaseValue = "100000",
            ActionName = "Join",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 40000000,
            BalanceValue = "40000000"
        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            IncreaseAmount = 200000,
            IncreaseValue = "200000",
            ActionName = "Increase",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 50000000,
            BalanceValue = "50000000"
        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            IncreaseAmount = 400000,
            IncreaseValue = "400000",
            ActionName = "Mint",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 60000000,
            BalanceValue = "60000000"
        });

        var pointsRecorded = new PointsChanged
        {
            PointsChangedDetails = pointsRecordList
        };

        var logEventContext = GenerateLogEventContext(pointsRecorded);
        await _pointsLogEventProcessor.ProcessAsync(logEventContext);

        var pointsSumByAction1 = await Query.GetPointsSumByAction(_actionIndexRepository, _objectMapper,
            new GetPointsSumByActionDto
            {
                Domain = "test.dapp.io",
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6"
            });
        pointsSumByAction1.TotalRecordCount.ShouldBe(2);


        var pointsSumByAction2 = await Query.GetPointsSumByAction(_actionIndexRepository, _objectMapper,
            new GetPointsSumByActionDto
            {
                Domain = "test.dapp.io",
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6",
                Role = IncomeSourceType.Kol
            });
        pointsSumByAction2.TotalRecordCount.ShouldBe(1);
        
        var addressLog = await Query.GetAddressPointLog(_logIndexRepository, _objectMapper,
            new GetAddressPointsLogDto
            {
                Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
                Role = IncomeSourceType.Kol
            });
        addressLog.TotalRecordCount.ShouldBe(1);
        
        var pointsSumBySymbol = await Query.GetPointsSumBySymbol(_symbolIndexRepository, _objectMapper,
            new GetPointsSumBySymbolDto
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                MaxResultCount = 10
            });
        pointsSumBySymbol.TotalRecordCount.ShouldBe(3);
    }
    
    [Fact]
    public async Task HandleQuery()
    {
        var pointsRecordList = new PointsChangedDetails();
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-1",
            IncreaseAmount = 100000,
            IncreaseValue = "100000",
            ActionName = "Join",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 40000000,
            BalanceValue = "40000000"
        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Kol,
            PointsName = "TEST-2",
            IncreaseAmount = 200000,
            IncreaseValue = "200000",
            ActionName = "Increase",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 50000000,
            BalanceValue = "50000000"
        });
        pointsRecordList.PointsDetails.Add(new PointsChangedDetail
        {
            Domain = "test.dapp.io",
            PointsReceiver = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            IncomeSourceType = IncomeSourceType.Inviter,
            PointsName = "TEST-6",
            IncreaseAmount = 400000,
            IncreaseValue = "400000",
            ActionName = "Mint",
            DappId = HashHelper.ComputeFrom("Schrodinger"),
            Balance = 60000000,
            BalanceValue = "60000000"
        });

        var pointsRecorded = new PointsChanged
        {
            PointsChangedDetails = pointsRecordList
        };

        var logEventContext = GenerateLogEventContext(pointsRecorded);
        await _pointsLogEventProcessor.ProcessAsync(logEventContext);

        var action = await Query.GetRankingList(_symbolIndexRepository, _objectMapper,
            new GetRankingListInput
            {
                DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6",
                SortingKeyWord = SortingKeywordType.SecondSymbolAmount,
                MaxResultCount = 10
            });
        action.TotalCount.ShouldBe(1);

        action = await Query.GetPointsEarnedList(_symbolIndexRepository, _objectMapper, new GetPointsEarnedListInput
        {
            DappId = "abface2803f57fa10f032baa58f30f748ef99c2b95e56f2a1b6a6e06faacc8f6",
            SortingKeyWord = SortingKeywordType.SecondSymbolAmount,
            Address = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ",
            Type = OperatorRole.Kol,
            MaxResultCount = 10
        });
        action.TotalCount.ShouldBe(1);

        var list = await Query.GetPointsRecordByName(_actionIndexRepository, _objectMapper,
            new GetPointsRecordByNameDto());
        list.TotalRecordCount.ShouldBe(3);
    }
}