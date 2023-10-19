using AutoMapper;
using Crawler.Core.BusinessLogics.BindingModels;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using System.Xml.Serialization;

namespace Crawler.Core.BusinessLogics.Helpers
{
    /// <summary>
    /// Вспомагательный класс для парсинга XML строки в нужный формат.
    /// </summary>
    public class XmlParseHelper
    {
        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        public XmlParseHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Запарсить строку в список справочной информации о валютах.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Объект CurrencyInfoListDTO.</returns>
        public CurrencyInfoListDTO ParseXmlCurrencyInfoToDTO(string xmlString)
        {
            List<CurrencyInfoDTO> itemList = new()
            {
                new CurrencyInfoDTO()
                {
                    RId = "R00000", // ( Я не знаю настоящий RId для рубля )
                    Name = "Рубль",
                    EngName = "Ruble",
                    ParentRId = "R00000",
                    ISOCharCode = "RUB"
                }
            };

            XmlSerializer serializer = new(typeof(ValutaListXMLBindingModel));
            ValutaListXMLBindingModel? currencyData;

            using (var reader = new StringReader(xmlString))
            {
                currencyData = (ValutaListXMLBindingModel?)serializer.Deserialize(reader);
            }

            List<CurrencyInfoDTO>? valuteInfoList = currencyData?.Items?.Select(valuteInfo => _mapper.Map<CurrencyInfoDTO>(valuteInfo)).ToList();

            if (valuteInfoList != null)
            {
                itemList.AddRange(valuteInfoList);
            }

            return new CurrencyInfoListDTO();
        }

        /// <summary>
        /// Запарсить строку в список информации о валютных котировках по датам.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Объект CurrencyValueByDateListDTO.</returns>
        public RubleQuotesByDateDTO ParseXmlCurrencyQuotesByDateToDTO(string xmlString)
        {
            List<RubleQuoteDTO> itemList = new();

            XmlSerializer serializer = new(typeof(ValCursListXMLBindingModel));
            ValCursListXMLBindingModel? currencyData;

            using (var reader = new StringReader(xmlString))
            {
                currencyData = (ValCursListXMLBindingModel?)serializer.Deserialize(reader);
            }

            List<RubleQuoteDTO>? currencyValuesList = currencyData?.Valutes?.Select(valuteValue => _mapper.Map<RubleQuoteDTO>(valuteValue)).ToList();

            if (currencyValuesList != null)
            {
                foreach (var valuteValue in currencyValuesList)
                {
                    valuteValue.BaseCurrencyId = "R00000";
                }

                itemList.AddRange(currencyValuesList);
            }

            return new RubleQuotesByDateDTO
            {
                Date = DateTime.ParseExact(currencyData?.Date ?? "01.01.0001", "dd.MM.yyyy", null),
                List = itemList
            };
        }
    }
}
