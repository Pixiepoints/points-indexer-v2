using AeFinder.Sdk.Entities;
using Nest;

namespace PixiePointsApp.Entities;

public class OperatorUserIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string Domain { get; set; }
    
    [Keyword] public string Address { get; set; }
    
    [Keyword] public string DappName { get; set; }

    public long CreateTime { get; set; }
}