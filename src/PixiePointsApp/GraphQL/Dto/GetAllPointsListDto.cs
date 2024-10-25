using Points.Contracts.Point;

namespace PixiePointsApp.GraphQL.Dto;

public class GetAllPointsListDto
{
    public string DappName { get; set; }
    public IncomeSourceType? Role { get; set; }
    public string LastId { get; set; }
    public long LastBlockHeight { get; set; }
    public int MaxResultCount { get; set; }
}