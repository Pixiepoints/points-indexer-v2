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

public class ReferralAcceptedLogEventProcessorTests : PixiePointsAppTestBase
{
    private readonly ReferralAcceptedLogEventProcessor _processor;
    private readonly IReadOnlyRepository<UserReferralRecordIndex> _recordRepository;
    private readonly IReadOnlyRepository<UserReferralCountIndex> _countRepository;
    private readonly IObjectMapper _objectMapper;

    public ReferralAcceptedLogEventProcessorTests()
    {
        _processor = GetRequiredService<ReferralAcceptedLogEventProcessor>();
        _recordRepository = GetRequiredService<IReadOnlyRepository<UserReferralRecordIndex>>();
        _countRepository = GetRequiredService<IReadOnlyRepository<UserReferralCountIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task HandleReferralAcceptedProcessor()
    {
        var referralAcceptedEvent1 = new ReferralAccepted
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            Referrer = Address.FromBase58("3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn"),
            DappId = HashHelper.ComputeFrom("Schrodinger")
        };

        var logEventContext = GenerateLogEventContext(referralAcceptedEvent1);
        await _processor.ProcessAsync(logEventContext);

        var records1 = await Query.GetUserReferralRecords(_recordRepository, _objectMapper,
            new GetUserReferralRecordsDto
            {
                ReferrerList =
                [
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                ],
                MaxResultCount = 10
            });
        records1.TotalRecordCount.ShouldBe(1);

        var counts1 = await Query.GetUserReferralCounts(_countRepository, _objectMapper,
            new GetUserReferralCountsDto
            {
                ReferrerList =
                [
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                ],
                MaxResultCount = 10
            });
        counts1.TotalRecordCount.ShouldBe(1);
        counts1.Data[0].InviteeNumber.ShouldBe(1);

        var referralAcceptedEvent2 = new ReferralAccepted
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("23GxsoW9TRpLqX1Z5tjrmcRMMSn5bhtLAf4HtPj8JX9BerqTqp"),
            Referrer = Address.FromBase58("3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn"),
            DappId = HashHelper.ComputeFrom("Schrodinger")
        };
        var logEventContext2 = GenerateLogEventContext(referralAcceptedEvent2);

        await _processor.ProcessAsync(logEventContext2);

        var records2 = await Query.GetUserReferralRecords(_recordRepository, _objectMapper,
            new GetUserReferralRecordsDto
            {
                ReferrerList =
                [
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                ],
                MaxResultCount = 10
            });
        records2.TotalRecordCount.ShouldBe(2);

        var counts2 = await Query.GetUserReferralCounts(_countRepository, _objectMapper,
            new GetUserReferralCountsDto
            {
                ReferrerList =
                [
                    "3yDz5oUiKHqhFJj1zPRmbLWFs9ErhGbWTwr9BR7uMXRMmJHMn",
                    "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"
                ],
                MaxResultCount = 10
            });
        counts2.TotalRecordCount.ShouldBe(1);
        counts2.Data[0].InviteeNumber.ShouldBe(2);
    }
}