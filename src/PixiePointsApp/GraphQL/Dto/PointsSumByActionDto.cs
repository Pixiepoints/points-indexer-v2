using Points.Contracts.Point;

namespace PixiePointsApp.GraphQL.Dto;

public class PointsSumByActionDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public IncomeSourceType Role { get; set; }
    public string DappId { get; set; }
    public string ActionName { get; set; } 
    public string Amount { get; set; }
    public string PointsName { get; set; } 

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class PointsSumByActionDtoList
{
    public long TotalRecordCount { get; set; }

    public List<PointsSumByActionDto> Data { get; set; }
}