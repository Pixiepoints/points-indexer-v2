using AeFinder.Sdk.Processor;
using AutoMapper;
using PixiePointsApp.Entities;
using PixiePointsApp.GraphQL.Dto;
using Points.Contracts.Point;

namespace PixiePointsApp;

public class PixiePointsAppAutoMapperProfile : Profile
{
    public PixiePointsAppAutoMapperProfile()
    {
        CreateMap<OperatorDomainIndex, OperatorDomainDto>();
        CreateMap<AddressPointsSumBySymbolIndex, PointsSumBySymbolDto>()
            .ForMember(t => t.FirstSymbolAmount, m => m.MapFrom(f => f.FirstSymbolAmount ?? "0"))
            .ForMember(t => t.SecondSymbolAmount, m => m.MapFrom(f => f.SecondSymbolAmount ?? "0"))
            .ForMember(t => t.ThirdSymbolAmount, m => m.MapFrom(f => f.ThirdSymbolAmount ?? "0"))
            .ForMember(t => t.FourSymbolAmount, m => m.MapFrom(f => f.FourSymbolAmount ?? "0"))
            .ForMember(t => t.FiveSymbolAmount, m => m.MapFrom(f => f.FiveSymbolAmount ?? "0"))
            .ForMember(t => t.SixSymbolAmount, m => m.MapFrom(f => f.SixSymbolAmount ?? "0"))
            .ForMember(t => t.SevenSymbolAmount, m => m.MapFrom(f => f.SevenSymbolAmount ?? "0"))
            .ForMember(t => t.EightSymbolAmount, m => m.MapFrom(f => f.EightSymbolAmount ?? "0"))
            .ForMember(t => t.NineSymbolAmount, m => m.MapFrom(f => f.NineSymbolAmount ?? "0"))
            .ForMember(t => t.TenSymbolAmount, m => m.MapFrom(f => f.TenSymbolAmount ?? "0"))
            .ForMember(t => t.ElevenSymbolAmount, m => m.MapFrom(f => f.ElevenSymbolAmount ?? "0"))
            .ForMember(t => t.TwelveSymbolAmount, m => m.MapFrom(f => f.TwelveSymbolAmount ?? "0"))
            .ForPath(t => t.BlockHeight, m => m.MapFrom(f => f.Metadata.Block.BlockHeight))
            ;
        CreateMap<AddressPointsSumByActionIndex, PointsSumByActionDto>();
        CreateMap<AddressPointsLogIndex, AddressPointsLogDto>();
        CreateMap<LogEventContext, OperatorUserIndex>().ReverseMap();
        CreateMap<OperatorUserIndex, OperatorUserDto>().ReverseMap();
        CreateMap<PointsChangedDetail, AddressPointsLogIndex>().ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).ForMember(destination => destination.Role,
            opt => opt.MapFrom(source => source.IncomeSourceType)).ForMember(destination => destination.DappId,
            opt => opt.MapFrom(source => source.DappId.ToHex()));
        CreateMap<PointsChangedDetail, AddressPointsSumByActionIndex>().ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).ForMember(destination => destination.Role,
            opt => opt.MapFrom(source => source.IncomeSourceType)).ForMember(destination => destination.DappId,
            opt => opt.MapFrom(source => source.DappId.ToHex()));

        CreateMap<PointsChangedDetail, AddressPointsSumBySymbolIndex>().ForMember(destination => destination.Address,
            opt => opt.MapFrom(source => source.PointsReceiver.ToBase58())).ForMember(destination => destination.Role,
            opt => opt.MapFrom(source => source.IncomeSourceType)).ForMember(destination => destination.DappId,
            opt => opt.MapFrom(source => source.DappId.ToHex()));
        
        CreateMap<LogEventContext, OperatorDomainIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsSumBySymbolIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsSumByActionIndex>().ReverseMap();
        CreateMap<LogEventContext, AddressPointsLogIndex>().ReverseMap();
        CreateMap<LogEventContext, OperatorDomainIndex>().ReverseMap();

        CreateMap<LogEventContext, UserReferralRecordIndex>().ReverseMap();
        CreateMap<LogEventContext, UserReferralCountIndex>().ReverseMap();
        CreateMap<UserReferralRecordIndex, UserReferralRecordsDto>().ReverseMap();
        CreateMap<UserReferralCountIndex, UserReferralCountsDto>().ReverseMap();

        CreateMap<AddressPointsSumBySymbolIndex, PointsSumDto>()
            .ForMember(t => t.UpdateTime, m => m.MapFrom(f => f.UpdateTime.ToUtcMilliSeconds()))
            .ForMember(t => t.DappName, m => m.MapFrom(f => f.DappId))
            .ForMember(t => t.FirstSymbolAmount, m => m.MapFrom(f => f.FirstSymbolAmount ?? "0"))
            .ForMember(t => t.SecondSymbolAmount, m => m.MapFrom(f => f.SecondSymbolAmount ?? "0"))
            .ForMember(t => t.ThirdSymbolAmount, m => m.MapFrom(f => f.ThirdSymbolAmount ?? "0"))
            .ForMember(t => t.FourSymbolAmount, m => m.MapFrom(f => f.FourSymbolAmount ?? "0"))
            .ForMember(t => t.FiveSymbolAmount, m => m.MapFrom(f => f.FiveSymbolAmount ?? "0"))
            .ForMember(t => t.SixSymbolAmount, m => m.MapFrom(f => f.SixSymbolAmount ?? "0"))
            .ForMember(t => t.SevenSymbolAmount, m => m.MapFrom(f => f.SevenSymbolAmount ?? "0"))
            .ForMember(t => t.EightSymbolAmount, m => m.MapFrom(f => f.EightSymbolAmount ?? "0"))
            .ForMember(t => t.NineSymbolAmount, m => m.MapFrom(f => f.NineSymbolAmount ?? "0"))
            .ForMember(t => t.TenSymbolAmount, m => m.MapFrom(f => f.TenSymbolAmount ?? "0"))
            .ForMember(t => t.ElevenSymbolAmount, m => m.MapFrom(f => f.ElevenSymbolAmount ?? "0"))
            .ForMember(t => t.TwelveSymbolAmount, m => m.MapFrom(f => f.TwelveSymbolAmount ?? "0"));
    }
}