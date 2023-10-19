namespace ExchangeData.DTOModels.CrawlerToStorage
{
    /// <summary>
    /// Модель списка справочных информаций о валютах.
    /// </summary>
    public class CurrencyInfoListDTO
    {
        /// <summary>
        /// Список справочных информаций о валютах.
        /// </summary>
        public List<CurrencyInfoDTO> List { get; set; } = new List<CurrencyInfoDTO>();
    }
}
