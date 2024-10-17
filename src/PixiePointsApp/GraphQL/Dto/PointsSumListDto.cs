namespace PixiePointsApp.GraphQL.Dto;

public class PointsSumListDto
{
    public long TotalCount { get; set; }

    public List<PointsSumDto> Data { get; set; }
}

public class PointsSumDto
{
    public string Domain { get; set; }
    public string Address { get; set; }
    public string FirstSymbolAmount { get; set; }
    public string SecondSymbolAmount { get; set; }
    public string ThirdSymbolAmount { get; set; }
    public string FourSymbolAmount { get; set; }
    public string FiveSymbolAmount { get; set; }
    public string SixSymbolAmount { get; set; }
    public string SevenSymbolAmount { get; set; }
    public string EightSymbolAmount { get; set; }
    public string NineSymbolAmount { get; set; }
    public string TenSymbolAmount { get; set; } 
    public string ElevenSymbolAmount { get; set; } 
    public string TwelveSymbolAmount { get; set; } 
    public long UpdateTime { get; set; }
    public string DappName { get; set; }
    public string Icon { get; set; }
    public OperatorRole Role { get; set; }
}