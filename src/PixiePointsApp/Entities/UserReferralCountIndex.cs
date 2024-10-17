using AeFinder.Sdk.Entities;
using Nest;

namespace PixiePointsApp.Entities;

public class UserReferralCountIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    [Keyword] public string Referrer { get; set; }
    
    public long InviteeNumber { get; set; }

    public long CreateTime { get; set; }
    
    public long UpdateTime { get; set; }
}