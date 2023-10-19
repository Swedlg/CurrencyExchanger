namespace ExchangeData.DTOModels.ConvertToStorage
{
    /// <summary>
    /// DTO модель котировки пары валют по дате.
    /// </summary>
    public class CurrencyQuotesByDateDTO
    {
        /// <summary>
        /// RId Базовой валюты.
        /// </summary>
        public string BaseCurrencyId { get; set; } = String.Empty;

        /// <summary>
        /// RId Другой валюты.
        /// </summary>
        public string CurrencyId { get; set; } = String.Empty;

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
