using Volo.Abp.Application.Dtos;

namespace PixiePointsApp.GraphQL.Dto;

public class GetOperatorDomainListInput
{
    public List<string> AddressList { get; set; }
    public string DappId { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}