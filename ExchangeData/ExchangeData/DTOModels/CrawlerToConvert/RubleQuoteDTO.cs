namespace ExchangeData.DTOModels.CrawlerToConvert
{
    /// <summary>
    /// Модель значение валюты относительно рубля.
    /// </summary>
    public class RubleQuoteDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="baseCurrencyId">ID Базовой валюты. (Рубль).</param>
        /// <param name="currencyId">ID Валюты.</param>
        /// <param name="value">Значение курса валют на дату.</param>
        public RubleQuoteDTO(string baseCurrencyId, string currencyId, decimal value)
        {
            BaseCurrencyId = baseCurrencyId;
            CurrencyId = currencyId;
            Value = value;
        }

        /// <summary>
        /// ID Базовой валюты. (Рубль).
        /// </summary>
        public string BaseCurrencyId { get; set; }

        /// <summary>
        /// ID Валюты.
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public decimal Value { get; set; }
    }
}
