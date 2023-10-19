namespace Crawler.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Класс информации а валюте и ее котировке по дате.
    /// </summary>
    public class JsonCurrencyByDateDTO
    {
        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты на английском.
        /// </summary>
        public string EngName { get; set; } = String.Empty;

        /// <summary>
        /// ISO_Char_Code.
        /// </summary>
        public string IsoCharCode { get; set; } = String.Empty;

        /// <summary>
        /// Номинал.
        /// </summary>
        public decimal Nominal { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public decimal Value { get; set; }
    }
}
