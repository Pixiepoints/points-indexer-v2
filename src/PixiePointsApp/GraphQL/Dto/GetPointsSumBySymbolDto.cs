using Volo.Abp.Application.Dtos;

namespace PixiePointsApp.GraphQL.Dto;

public class GetPointsSumBySymbolDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string DappName { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}