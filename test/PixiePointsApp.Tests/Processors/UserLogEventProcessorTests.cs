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

public class UserLogEventProcessorTests : PixiePointsAppTestBase
{
    private readonly JoinedLogEventProcessor _processor;
    private readonly IReadOnlyRepository<OperatorUserIndex> _repository;
    private readonly IObjectMapper _objectMapper;

    public UserLogEventProcessorTests()
    {
        _processor = GetRequiredService<JoinedLogEventProcessor>();
        _repository = GetRequiredService<IReadOnlyRepository<OperatorUserIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public async Task HandleJoinedProcessor()
    {
        var joined = new Joined
        {
            Domain = "test.dapp.io",
            Registrant = Address.FromBase58("2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"),
            DappId = HashHelper.ComputeFrom("XUnit"),
        };
        
        var logEventContext = GenerateLogEventContext(joined);
        await _processor.ProcessAsync(logEventContext);
        
        var userPager = await Query.QueryUserAsync(_repository, _objectMapper, new OperatorUserRequestDto
        {
            MaxResultCount = 10
        });
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
        
        // Repeat execution, there is only one piece of data in the library.
        await _processor.ProcessAsync(logEventContext);
        
        userPager = await Query.QueryUserAsync(_repository, _objectMapper, new OperatorUserRequestDto
        {
            MaxResultCount = 10
        });
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
    }


    [Fact]
    public async Task HandleJoinedProcessorQuery()
    {
        await HandleJoinedProcessor();

        var ts = DateTime.UtcNow.ToUtcMilliSeconds();
        await Task.Delay(200);
        
        var joined = new Joined
        {
            Domain = "test.dapp.io",
            Registrant = Address.FromBase58("xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"),
            DappId = HashHelper.ComputeFrom("XUnit"),
        };
        
        var logEventContext = GenerateLogEventContext(joined);
        await _processor.ProcessAsync(logEventContext);
        
        var userPager = await Query.QueryUserAsync(_repository, _objectMapper, new OperatorUserRequestDto
        {
            DomainIn = ["test.dapp.io"],
            AddressIn = ["xsnQafDAhNTeYcooptETqWnYBksFGGXxfcQyJJ5tmu6Ak9ZZt"],
            MaxResultCount = 10
            // CreateTimeGtEq = ts
            // CreateTimeLt = ts
        });
        userPager.ShouldNotBeNull();
        userPager.TotalRecordCount.ShouldBe(1);
        userPager.Data.ShouldNotBeEmpty();
    }
}