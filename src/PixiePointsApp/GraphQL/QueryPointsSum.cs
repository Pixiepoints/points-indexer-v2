using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using PixiePointsApp.Entities;
using PixiePointsApp.GraphQL.Dto;
using Points.Contracts.Point;
using Volo.Abp.ObjectMapping;

namespace PixiePointsApp.GraphQL;

public partial class Query
{
    [Name("getRankingList")]
    public static async Task<PointsSumListDto> GetRankingList(
        [FromServices] IReadOnlyRepository<AddressPointsSumBySymbolIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetRankingListInput input)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!input.Keyword.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.Domain == input.Keyword || i.Address == input.Keyword);
        }

        queryable = queryable.Where(i => i.DappId == input.DappId && i.Role == IncomeSourceType.Kol);

        queryable = queryable.Where(
            DomainInfoConstants.InternalDomains.Select(domain =>
                    (Expression<Func<AddressPointsSumBySymbolIndex, bool>>)(o => !o.Domain.Contains(domain)))
                .Aggregate((prev, next) => prev.Or(next)));
        
        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new PointsSumListDto
            {
                Data = []
            };
        }

        var recordList = SortByPropertyName(queryable, input.SortingKeyWord, input.Sorting)
            .Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new PointsSumListDto
        {
            Data = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumDto>>(recordList),
            TotalCount = totalCount
        };
    }

    [Name("getPointsEarnedList")]
    public static async Task<PointsSumListDto> GetPointsEarnedList(
        [FromServices] IReadOnlyRepository<AddressPointsSumBySymbolIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetPointsEarnedListInput input)
    {
        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(i => i.Address == input.Address && i.DappId == input.DappId);

        queryable = input.Type == OperatorRole.All
            ? queryable.Where(i => i.Role == IncomeSourceType.Kol || i.Role == IncomeSourceType.Inviter)
            : queryable.Where(i => (int)i.Role == (int)input.Type);
        
        queryable = queryable.Where(
            DomainInfoConstants.InternalDomains.Select(domain =>
                    (Expression<Func<AddressPointsSumBySymbolIndex, bool>>)(o => !o.Domain.Contains(domain)))
                .Aggregate((prev, next) => prev.Or(next)));

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new PointsSumListDto
            {
                Data = []
            };
        }

        var recordList = SortByPropertyName(queryable, input.SortingKeyWord, input.Sorting)
            .Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new PointsSumListDto
        {
            Data = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumDto>>(recordList),
            TotalCount = totalCount
        };
    }

    [Name("getAllPointsList")]
    public static async Task<PointsSumBySymbolDtoList> GetAllPointsList(
        [FromServices] IReadOnlyRepository<AddressPointsSumBySymbolIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetAllPointsListDto input)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!string.IsNullOrEmpty(input.DappName))
        {
            queryable = queryable.Where(i => i.DappId == input.DappName);
        }

        if (input.Role == null)
        {
            queryable = queryable.Where(i => i.Role == IncomeSourceType.User);
        }

        var totalCount = queryable.Count();
        if (totalCount == 0)
            return new PointsSumBySymbolDtoList
            {
                Data = []
            };

        if (string.IsNullOrEmpty(input.LastId) || input.LastBlockHeight > 0)
        {
        }

        var recordList = string.IsNullOrEmpty(input.LastId) || input.LastBlockHeight == 0
            ? queryable
                .OrderBy(o => o.Metadata.Block.BlockHeight)
                .ThenBy(o => o.Id)
                .Take(input.MaxResultCount)
                .ToList()
            : queryable
                .OrderBy(o => o.Metadata.Block.BlockHeight)
                .ThenBy(o => o.Id)
                .After([input.LastBlockHeight, input.LastId])
                .Take(input.MaxResultCount)
                .ToList();
        
        return new PointsSumBySymbolDtoList
        {
            Data = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumBySymbolDto>>(recordList),
            TotalRecordCount = totalCount
        };
    }

    private static IEnumerable<AddressPointsSumBySymbolIndex> SortByPropertyName(
        IEnumerable<AddressPointsSumBySymbolIndex> list, SortingKeywordType sortingKeywordType, string sortType)
    {
        return sortingKeywordType switch
        {
            SortingKeywordType.FirstSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.FirstSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.FirstSymbolAmount)),
            SortingKeywordType.SecondSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.SecondSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.SecondSymbolAmount)),
            SortingKeywordType.ThirdSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.ThirdSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.ThirdSymbolAmount)),
            SortingKeywordType.FourSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.FourSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.FourSymbolAmount)),
            SortingKeywordType.FiveSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.FiveSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.FiveSymbolAmount)),
            SortingKeywordType.SixSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.SixSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.SixSymbolAmount)),
            SortingKeywordType.SevenSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.SevenSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.SevenSymbolAmount)),
            SortingKeywordType.EightSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.EightSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.EightSymbolAmount)),
            SortingKeywordType.NineSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.NineSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.NineSymbolAmount)),
            SortingKeywordType.TenSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.TenSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.TenSymbolAmount)),
            SortingKeywordType.ElevenSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.ElevenSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.ElevenSymbolAmount)),
            SortingKeywordType.TwelveSymbolAmount => sortType == "DESC"
                ? list.OrderByDescending(x => ParseDouble(x.TwelveSymbolAmount))
                : list.OrderBy(x => ParseDouble(x.TwelveSymbolAmount)),
            _ => list
        };
    }

    private static double ParseDouble(string input)
    {
        return double.TryParse(input, out var result) ? result : 0;
    }
}