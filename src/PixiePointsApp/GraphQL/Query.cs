using System.Linq.Expressions;
using AeFinder.Sdk;
using AElf;
using GraphQL;
using PixiePointsApp.Entities;
using PixiePointsApp.GraphQL.Dto;
using Points.Contracts.Point;
using Volo.Abp.ObjectMapping;

namespace PixiePointsApp.GraphQL;

public partial class Query
{
    [Name("operatorDomainInfo")]
    public static async Task<OperatorDomainDto> OperatorDomainInfo(
        [FromServices] IReadOnlyRepository<OperatorDomainIndex> repository, [FromServices] IObjectMapper objectMapper,
        GetOperatorDomainDto input)
    {
        if (input.Domain.IsNullOrWhiteSpace()) return new OperatorDomainDto();
        var id = HashHelper.ComputeFrom(input.Domain).ToHex();

        var queryable = await repository.GetQueryableAsync();
        var domainIndex = queryable.SingleOrDefault(i => i.Id == id);

        return domainIndex == null
            ? new OperatorDomainDto()
            : objectMapper.Map<OperatorDomainIndex, OperatorDomainDto>(domainIndex);
    }

    [Name("checkDomainApplied")]
    public static async Task<DomainAppliedDto> CheckDomainApplied(
        [FromServices] IReadOnlyRepository<OperatorDomainIndex> repository, CheckDomainAppliedDto input)
    {
        if (input.DomainList.IsNullOrEmpty()) return new DomainAppliedDto();

        var queryable = await repository.GetQueryableAsync();

        var domainIndexList = queryable.Where(
            input.DomainList.Select(domain =>
                    (Expression<Func<OperatorDomainIndex, bool>>)(o => o.Domain.Contains(domain)))
                .Aggregate((prev, next) => prev.Or(next))).ToList();

        if (domainIndexList.Count == 0) return new DomainAppliedDto();

        return new DomainAppliedDto
        {
            DomainList = domainIndexList.Select(i => i.Domain).ToList()
        };
    }

    [Name("getPointsSumBySymbol")]
    public static async Task<PointsSumBySymbolDtoList> GetPointsSumBySymbol(
        [FromServices] IReadOnlyRepository<AddressPointsSumBySymbolIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetPointsSumBySymbolDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartTime != DateTime.MinValue)
        {
            queryable = queryable.Where(i => i.UpdateTime >= input.StartTime);
        }

        if (input.EndTime != DateTime.MinValue)
        {
            queryable = queryable.Where(i => i.UpdateTime < input.EndTime);
        }

        if (!string.IsNullOrEmpty(input.DappName))
        {
            queryable = queryable.Where(i => i.DappId == input.DappName);
        }

        if (!input.AddressList.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.AddressList.Select(address =>
                        (Expression<Func<AddressPointsSumBySymbolIndex, bool>>)(o => o.Address.Contains(address)))
                    .Aggregate((prev, next) => prev.Or(next)));
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

        var recordList = queryable.OrderBy(o => o.UpdateTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new PointsSumBySymbolDtoList
        {
            Data = objectMapper.Map<List<AddressPointsSumBySymbolIndex>, List<PointsSumBySymbolDto>>(recordList),
            TotalRecordCount = totalCount
        };
    }

    [Name("getPointsSumByAction")]
    public static async Task<PointsSumByActionDtoList> GetPointsSumByAction(
        [FromServices] IReadOnlyRepository<AddressPointsSumByActionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetPointsSumByActionDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.DappId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.DappId == input.DappId);
        }

        if (!input.Address.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.Address == input.Address);
        }

        if (!input.Domain.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.Domain == input.Domain);
        }

        if (input.Role != null)
        {
            queryable = queryable.Where(i => i.Role == input.Role);
        }

        var totalCount = queryable.Count();
        if (totalCount == 0)
            return new PointsSumByActionDtoList
            {
                Data = []
            };

        return new PointsSumByActionDtoList
        {
            Data =
                objectMapper.Map<List<AddressPointsSumByActionIndex>, List<PointsSumByActionDto>>(queryable.ToList()),
            TotalRecordCount = totalCount
        };
    }

    [Name("getPointsRecordByName")]
    public static async Task<PointsSumByActionDtoList> GetPointsRecordByName(
        [FromServices] IReadOnlyRepository<AddressPointsSumByActionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetPointsRecordByNameDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.DappId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.DappId == input.DappId);
        }

        if (!input.Address.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.Address == input.Address);
        }

        if (!input.PointsName.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(i => i.PointsName == input.PointsName);
        }

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new PointsSumByActionDtoList
            {
                Data = []
            };
        }

        return new PointsSumByActionDtoList
        {
            Data =
                objectMapper.Map<List<AddressPointsSumByActionIndex>, List<PointsSumByActionDto>>(queryable.ToList()),
            TotalRecordCount = totalCount
        };
    }

    [Name("getAddressPointLog")]
    public static async Task<AddressPointsLogDtoList> GetAddressPointLog(
        [FromServices] IReadOnlyRepository<AddressPointsLogIndex> repository, [FromServices] IObjectMapper objectMapper,
        GetAddressPointsLogDto input)
    {
        var queryable = await repository.GetQueryableAsync();
        var recordList = queryable.Where(i => i.Role == input.Role && i.Address == input.Address).ToList();

        if (recordList.Count == 0)
        {
            return new AddressPointsLogDtoList
            {
                Data = []
            };
        }

        return new AddressPointsLogDtoList
        {
            Data = objectMapper.Map<List<AddressPointsLogIndex>, List<AddressPointsLogDto>>(recordList),
            TotalRecordCount = recordList.Count
        };
    }


    [Name("getUserReferralRecords")]
    public static async Task<UserReferralRecordDtoList> GetUserReferralRecords(
        [FromServices] IReadOnlyRepository<UserReferralRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetUserReferralRecordsDto input)
    {
        if (input.ReferrerList.IsNullOrEmpty())
        {
            return new UserReferralRecordDtoList
            {
                Data = []
            };
        }

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(
            input.ReferrerList.Select(referrer =>
                    (Expression<Func<UserReferralRecordIndex, bool>>)(u => u.Referrer.Contains(referrer)))
                .Aggregate((prev, next) => prev.Or(next)));

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new UserReferralRecordDtoList
            {
                Data = []
            };
        }

        var recordList = queryable.OrderBy(i => i.CreateTime)
            .Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new UserReferralRecordDtoList
        {
            Data = objectMapper.Map<List<UserReferralRecordIndex>, List<UserReferralRecordsDto>>(recordList),
            TotalRecordCount = totalCount
        };
    }


    [Name("getUserReferralCounts")]
    public static async Task<UserReferralCountDtoList> GetUserReferralCounts(
        [FromServices] IReadOnlyRepository<UserReferralCountIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetUserReferralCountsDto input)
    {
        if (input.ReferrerList.IsNullOrEmpty())
        {
            return new UserReferralCountDtoList
            {
                Data = []
            };
        }

        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(
            input.ReferrerList.Select(referrer =>
                    (Expression<Func<UserReferralCountIndex, bool>>)(u => u.Referrer.Contains(referrer)))
                .Aggregate((prev, next) => prev.Or(next)));

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new UserReferralCountDtoList
            {
                Data = []
            };
        }

        var recordList = queryable.OrderBy(i => i.CreateTime)
            .Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new UserReferralCountDtoList
        {
            Data = objectMapper.Map<List<UserReferralCountIndex>, List<UserReferralCountsDto>>(recordList),
            TotalRecordCount = totalCount
        };
    }

    [Name("getOperatorDomainList")]
    public static async Task<OperatorDomainListDto> GetOperatorDomainList(
        [FromServices] IReadOnlyRepository<OperatorDomainIndex> repository, [FromServices] IObjectMapper objectMapper,
        GetOperatorDomainListInput input)
    {
        if (input.AddressList.IsNullOrEmpty())
        {
            return new OperatorDomainListDto
            {
                Data = []
            };
        }

        var queryable = await repository.GetQueryableAsync();

        queryable = queryable.Where(i => i.DappId == input.DappId);

        queryable = queryable.Where(
            input.AddressList.Select(address =>
                    (Expression<Func<OperatorDomainIndex, bool>>)(u => u.InviterAddress.Contains(address)))
                .Aggregate((prev, next) => prev.Or(next)));

        var totalCount = queryable.Count();
        if (totalCount == 0)
        {
            return new OperatorDomainListDto
            {
                Data = []
            };
        }

        var recordList = queryable.OrderBy(i => i.CreateTime)
            .Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        return new OperatorDomainListDto
        {
            Data = objectMapper.Map<List<OperatorDomainIndex>, List<OperatorDomainDto>>(recordList),
            TotalRecordCount = totalCount
        };
    }
}