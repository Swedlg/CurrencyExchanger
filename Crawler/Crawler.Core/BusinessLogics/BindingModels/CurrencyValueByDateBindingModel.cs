namespace Crawler.Core.BindingModels
{
    /// <summary>
    /// Модель информации о валютной котировке
    /// </summary>
    public class CurrencyValueByDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateOnly Date {  get; set; }

        /// <summary>
        /// Цифровой код валюты.
        /// </summary>
        public string NumCode { get; set; }

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        public string CharCode { get; set; }

        /// <summary>
        /// Номинал валюты.
        /// </summary>
        public string Nominal { get; set; }

        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение стоимости валюты.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Значение стоимости валюты соотносительно с номиналом.
        /// </summary>
        public string VunitRate { get; set; }
    }
}
