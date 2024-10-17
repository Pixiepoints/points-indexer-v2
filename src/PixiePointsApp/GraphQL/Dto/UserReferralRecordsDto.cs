namespace PixiePointsApp.GraphQL.Dto;


public class UserReferralRecordsDto
{
    public string Domain { get; set; }
    
    public string DappId { get; set; }  
    
    public string Referrer { get; set; }
    
    public string Invitee { get; set; }
    
    public string Inviter { get; set; }

    public long CreateTime { get; set; }
}

public class UserReferralRecordDtoList
{
    public long TotalRecordCount { get; set; }

    public List<UserReferralRecordsDto> Data { get; set; }
}