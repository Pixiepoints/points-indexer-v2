using Points.Contracts.Point;

namespace PixiePointsApp.GraphQL.Dto;

public class GetPointsSumBySymbolDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string DappName { get; set; }
    public List<string> AddressList { get; set; }
    public bool? HiddenMainDomain { get; set; }
    public IncomeSourceType? Role { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}