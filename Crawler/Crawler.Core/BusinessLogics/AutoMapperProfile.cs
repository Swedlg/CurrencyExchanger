using AutoMapper;
using Crawler.Core.BindingModels;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;

namespace Crawler.Core.BusinessLogics
{
    /// <summary>
    /// Профиль AutoMapper'а для конвертации BindingModel'ей в DTO модели.
    /// </summary>
    public  class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<CurrencyInfoXMLBindingModel, CurrencyInfoDTO>()
                .ForMember(cdto => cdto.RId, cm => cm.MapFrom(src => src.Id))
                .ForMember(cdto => cdto.Name, cm => cm.MapFrom(src => src.Name))
                .ForMember(cdto => cdto.EngName, cm => cm.MapFrom(src => src.EngName))
                .ForMember(cdto => cdto.ParentRId, cm => cm.MapFrom(src => src.ParentCode))
                .ForMember(cdto => cdto.ISOCharCode, cm => cm.MapFrom(src => src.IsoCharCode));

            CreateMap<RubleQuoteByDateXMLBindingModel, RubleQuoteDTO>()
                .ForMember(cdto => cdto.CurrencyId, cm => cm.MapFrom(src => src.Id))
                .ForMember(cdto => cdto.Value, cm => cm.MapFrom(src => src.VunitRate));
        }
    }
}
