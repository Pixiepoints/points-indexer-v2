using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using PixiePointsApp.Entities;
using PixiePointsApp.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace PixiePointsApp.GraphQL;

public partial class Query
{
    [Name("QueryUserAsync")]
    public static async Task<OperatorUserPagerDto> QueryUserAsync(
        [FromServices] IReadOnlyRepository<OperatorUserIndex> repository, [FromServices] IObjectMapper objectMapper,
        OperatorUserRequestDto input)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!input.DomainIn.IsNullOrEmpty())
            queryable = queryable.Where(
                input.DomainIn.Select(domain =>
                        (Expression<Func<OperatorUserIndex, bool>>)(o => o.Domain.Contains(domain)))
                    .Aggregate((prev, next) => prev.Or(next)));

        if (!input.AddressIn.IsNullOrEmpty())
            queryable = queryable.Where(
                input.AddressIn.Select(address =>
                        (Expression<Func<OperatorUserIndex, bool>>)(o => o.Address.Contains(address)))
                    .Aggregate((prev, next) => prev.Or(next)));

        if (!input.DappNameIn.IsNullOrEmpty())
            queryable = queryable.Where(
                input.DappNameIn.Select(dappName =>
                        (Expression<Func<OperatorUserIndex, bool>>)(o => o.DappName.Contains(dappName)))
                    .Aggregate((prev, next) => prev.Or(next)));

        if (input.CreateTimeLt != null)
            queryable = queryable.Where(i => i.CreateTime < input.CreateTimeLt);

        if (input.CreateTimeGtEq != null)
            queryable = queryable.Where(i => i.CreateTime >= input.CreateTimeGtEq);
        
        queryable = queryable.Where(
            DomainInfoConstants.InternalDomains.Select(domain =>
                    (Expression<Func<OperatorUserIndex, bool>>)(o => !o.Domain.Contains(domain)))
                .Aggregate((prev, next) => prev.Or(next)));

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new OperatorUserPagerDto
            {
                TotalRecordCount = 0,
                Data = []
            };
        }

        var recordList = queryable.OrderByDescending(i => i.CreateTime).Skip(input.SkipCount).Take(input.MaxResultCount)
            .ToList();

        return new OperatorUserPagerDto
        {
            TotalRecordCount = totalCount,
            Data = objectMapper.Map<List<OperatorUserIndex>, List<OperatorUserDto>>(recordList)
        };
    }
}