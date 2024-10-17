namespace PixiePointsApp.GraphQL.Dto;

public class GetPointsRecordByNameDto
{
    public string DappId { get; set; }
    public string Address { get; set; }
    public string PointsName { get; set; }
}