using Volo.Abp.Application.Dtos;

namespace PixiePointsApp.GraphQL.Dto;

public class GetUserReferralRecordsDto
{
    public List<string> ReferrerList { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}