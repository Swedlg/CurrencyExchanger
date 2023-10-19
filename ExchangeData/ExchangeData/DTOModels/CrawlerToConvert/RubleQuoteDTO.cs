namespace ExchangeData.DTOModels.CrawlerToConvert
{
    /// <summary>
    /// Модель значение валюты относительно рубля.
    /// </summary>
    public class RubleQuoteDTO
    {
        /// <summary>
        /// ID Базовой валюты. (Рубль).
        /// </summary>
        public string BaseCurrencyId { get; set; } = String.Empty;

        /// <summary>
        /// ID Валюты.
        /// </summary>
        public string CurrencyId { get; set; } = String.Empty;

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public decimal Value { get; set; }
    }
}
