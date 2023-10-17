namespace ExchangeData.DTOModels.CrawlerToStorage
{
    /// <summary>
    /// Модель списка справочных информаций о валютах.
    /// </summary>
    public class CurrencyInfoListDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="list">Список справочных информаций о валютах.</param>
        public CurrencyInfoListDTO(List<CurrencyInfoDTO> list)
        {
            List = list;
        }

        /// <summary>
        /// Список справочных информаций о валютах.
        /// </summary>
        public List<CurrencyInfoDTO> List { get; set; }
    }
}
