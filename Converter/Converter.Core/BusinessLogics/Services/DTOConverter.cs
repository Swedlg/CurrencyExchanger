using ExchangeData.DTOModels.ConvertToStorage;
using ExchangeData.DTOModels.CrawlerToConvert;

namespace Converter.Core.BusinessLogics.Services
{
    /// <summary>
    /// Ковертер DTO объектов об котировках валют по датам.
    /// </summary>
    public static class DTOConverter
    {
        /// <summary>
        /// Запарсить список валют относительно рубля по дате в список всех валют относительно друг друга по дате.
        /// </summary>
        /// <param name="list">Список валют относительно рубля</param>
        /// <param name="date">Дата.</param>
        /// <returns>Список всех валют относительно друг друга по дате</returns>
        public static CurrencyQuotesByDateListDTO ConvertToCurrencyQuotesByDateListDTO(List<RubleQuoteDTO> list, DateTime date)
        {
            List<CurrencyQuotesByDateDTO> listOfcurrencyQuotesByDateDTO = new();

            foreach (RubleQuoteDTO rubleQuoteDTO in list)
            {
                listOfcurrencyQuotesByDateDTO.Add(new CurrencyQuotesByDateDTO()
                {
                    BaseCurrencyId = rubleQuoteDTO.BaseCurrencyId,
                    CurrencyId = rubleQuoteDTO.CurrencyId,
                    Date = date,
                    Value = rubleQuoteDTO.Value,
                });
            }

            foreach (RubleQuoteDTO rubleQuoteDTO1 in list)
            {
                foreach (RubleQuoteDTO rubleQuoteDTO2 in list)
                {
                    if (rubleQuoteDTO1.CurrencyId != rubleQuoteDTO2.CurrencyId)
                    {
                        listOfcurrencyQuotesByDateDTO.Add(new CurrencyQuotesByDateDTO()
                        {
                            BaseCurrencyId = rubleQuoteDTO1.BaseCurrencyId,
                            CurrencyId = rubleQuoteDTO2.CurrencyId,
                            Date = date,
                            Value = rubleQuoteDTO2.Value / rubleQuoteDTO1.Value,
                        });
                    }
                }
            }

            return new CurrencyQuotesByDateListDTO() { List = listOfcurrencyQuotesByDateDTO };
        }
    }
}
