using AeFinder.Sdk.Entities;
using Nest;

namespace PixiePointsApp.Entities;

public class OperatorDomainIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }  
    
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string DepositAddress { get; set; }
    
    [Keyword] public string InviterAddress { get; set; }
    
    [Keyword] public string DappId { get; set; }  
    
    public DateTime CreateTime { get; set; }  
}