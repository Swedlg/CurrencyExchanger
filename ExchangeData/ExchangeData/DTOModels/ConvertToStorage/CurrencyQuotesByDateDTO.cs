namespace ExchangeData.DTOModels.ConvertToStorage
{
    /// <summary>
    /// DTO модель котировки пары валют по дате.
    /// </summary>
    public class CurrencyQuotesByDateDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="baseCurrencyId">RId Базовой валюты.</param>
        /// <param name="currencyId">RId Другой валюты.</param>
        /// <param name="date">Дата.</param>
        /// <param name="value">Значение котировки.</param>
        public CurrencyQuotesByDateDTO(string baseCurrencyId, string currencyId, DateTime date, decimal value)
        {
            BaseCurrencyId = baseCurrencyId;
            CurrencyId = currencyId;
            Date = date;
            Value = value;
        }

        /// <summary>
        /// RId Базовой валюты.
        /// </summary>
        public string BaseCurrencyId { get; set; }

        /// <summary>
        /// RId Другой валюты.
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Значение котировки.
        /// </summary>
        public decimal Value { get; set; }
    }
}
