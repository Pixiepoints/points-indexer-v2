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

public class AppliedLogEventProcessorTests : PixiePointsAppTestBase
{
    private readonly AppliedLogEventProcessor _processor;
    private readonly IReadOnlyRepository<OperatorDomainIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public AppliedLogEventProcessorTests()
    {
        _processor = GetRequiredService<AppliedLogEventProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<OperatorDomainIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task HandleInviterAppliedProcessor()
    {
        var joined = new InviterApplied
        {
            Domain = "test.dapp.io",
            Inviter = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            Invitee = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            DappId = HashHelper.ComputeFrom("Schrodinger"),
        };

        var logEventContext = GenerateLogEventContext(joined);
        await _processor.ProcessAsync(logEventContext);
        
        var domain = await Query.CheckDomainApplied(_repository, new CheckDomainAppliedDto
        {
            DomainList =
            [
                "test3.dapp.io",
                "test.dapp.io",
                "test2.dapp.io"
            ]
        });
        domain.DomainList.ShouldContain("test.dapp.io");
        domain.DomainList.ShouldNotContain("test2.dapp.io");

        var info = await Query.OperatorDomainInfo(_repository, _objectMapper, new GetOperatorDomainDto
        {
            Domain = "test.dapp.io",
        });
        info.DepositAddress.ShouldBe("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt");
        
        var list = await Query.GetOperatorDomainList(_repository, _objectMapper, new GetOperatorDomainListInput
        {
            AddressList = ["2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"],
            DappId = HashHelper.ComputeFrom("Schrodinger").ToHex(),
            MaxResultCount = 10
        });
        list.TotalRecordCount.ShouldBe(1);
    }
}