namespace ExchangeData.DTOModels
{
    /// <summary>
    /// Значение валюты по дате.
    /// </summary>
    public class CurrencyValueByDateDTO
    {
        public CurrencyValueByDateDTO(string baseCurrencyId, string currencyId, decimal value)
        {
            BaseCurrencyId = baseCurrencyId;
            CurrencyId = currencyId;
            Value = value;
        }

        /// <summary>
        /// ID Базовой валюты.
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
