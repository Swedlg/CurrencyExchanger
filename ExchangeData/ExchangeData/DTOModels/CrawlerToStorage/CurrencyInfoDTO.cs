namespace ExchangeData.DTOModels.CrawlerToStorage
{
    /// <summary>
    /// Модель справочной информации о валюте.
    /// </summary>
    public class CurrencyInfoDTO
    {
        /// <summary>
        /// RId.
        /// </summary>
        public string RId { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public string EngName { get; set; } = String.Empty;

        /// <summary>
        /// Код родительской валюты.
        /// </summary>
        public string ParentRId { get; set; } = String.Empty;

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        public string ISOCharCode { get; set; } = String.Empty;
    }
}
