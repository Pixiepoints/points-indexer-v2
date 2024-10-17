using AeFinder.Sdk.Entities;
using Nest;

namespace PixiePointsApp.Entities;

public class UserReferralRecordIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    [Keyword] public string Referrer { get; set; }
    
    [Keyword] public string Invitee { get; set; }
    
    [Keyword] public string Inviter { get; set; }

    public long CreateTime { get; set; }
}