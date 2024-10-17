using Volo.Abp.Application.Dtos;

namespace PixiePointsApp.GraphQL.Dto;

public class GetRankingListInput
{
    public string Keyword { get; set; }
    public string DappId { get; set; }
    public SortingKeywordType SortingKeyWord { get; set; }
    public string Sorting { get; set; } = "DESC";
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}

public enum SortingKeywordType
{
    FirstSymbolAmount,
    SecondSymbolAmount,
    ThirdSymbolAmount,
    FourSymbolAmount,
    FiveSymbolAmount,
    SixSymbolAmount,
    SevenSymbolAmount,
    EightSymbolAmount,
    NineSymbolAmount,
    TenSymbolAmount,
    ElevenSymbolAmount,
    TwelveSymbolAmount
}