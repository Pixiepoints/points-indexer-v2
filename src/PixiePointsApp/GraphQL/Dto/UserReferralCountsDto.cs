namespace PixiePointsApp.GraphQL.Dto;

public class UserReferralCountsDto
{
    public string Domain { get; set; }
    
    public string DappId { get; set; }  
    
    public string Referrer { get; set; }
    
    public long InviteeNumber { get; set; }

    public long CreateTime { get; set; }
    
    public long UpdateTime { get; set; }
}

public class UserReferralCountDtoList
{
    public long TotalRecordCount { get; set; }

    public List<UserReferralCountsDto> Data { get; set; }
}