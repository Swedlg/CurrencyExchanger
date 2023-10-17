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
            List<CurrencyQuotesByDateDTO> listOfcurrencyQuotesByDateDTO = new List<CurrencyQuotesByDateDTO>();

            foreach (RubleQuoteDTO rubleQuoteDTO in list)
            {
                listOfcurrencyQuotesByDateDTO.Add(new CurrencyQuotesByDateDTO(
                                                        rubleQuoteDTO.BaseCurrencyId,
                                                        rubleQuoteDTO.CurrencyId,
                                                        date,
                                                        rubleQuoteDTO.Value));
            }

            foreach (RubleQuoteDTO rubleQuoteDTO1 in list)
            {
                foreach (RubleQuoteDTO rubleQuoteDTO2 in list)
                {
                    if (rubleQuoteDTO1.CurrencyId != rubleQuoteDTO2.CurrencyId)
                    {
                        listOfcurrencyQuotesByDateDTO.Add(new CurrencyQuotesByDateDTO(
                            rubleQuoteDTO1.CurrencyId,
                            rubleQuoteDTO2.CurrencyId,
                            date,
                            rubleQuoteDTO2.Value / rubleQuoteDTO1.Value));
                    }
                }
            }

            return new CurrencyQuotesByDateListDTO(listOfcurrencyQuotesByDateDTO);
        }
    }
}
