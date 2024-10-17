namespace PixiePointsApp.GraphQL.Dto;

public class OperatorDomainDto
{
    public  string Id { get; set; }  
    
    public string Domain { get; set; }
    
    public string DepositAddress { get; set; }
    
    public string InviterAddress { get; set; }
    
    public string DappId { get; set; }  
    
    public DateTime CreateTime { get; set; }  
}

public class OperatorDomainListDto
{
    public long TotalRecordCount { get; set; }
    public List<OperatorDomainDto> Data { get; set; }
}