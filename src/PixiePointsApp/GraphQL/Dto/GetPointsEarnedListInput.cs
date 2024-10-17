using Volo.Abp.Application.Dtos;

namespace PixiePointsApp.GraphQL.Dto;

public class GetPointsEarnedListInput
{
    public string Sorting { get; set; } = "DESC";
    public string Address { get; set; }
    public string DappId { get; set; }
    public OperatorRole Type { get; set; }
    public SortingKeywordType SortingKeyWord { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}

public enum OperatorRole
{
    Inviter = 2,
    Kol = 1,
    User = 0,
    All = -1
}