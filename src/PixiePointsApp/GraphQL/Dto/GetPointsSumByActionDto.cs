using Points.Contracts.Point;

namespace PixiePointsApp.GraphQL.Dto;

public class GetPointsSumByActionDto
{ 
    public string DappId { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType? Role { get; set; }
}