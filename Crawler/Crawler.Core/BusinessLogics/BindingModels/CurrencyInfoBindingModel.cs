namespace Crawler.Core.BindingModels
{
    /// <summary>
    /// Модель информации о валюте.
    /// </summary>
    public class CurrencyInfoBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название валюты на английском.
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// Номинал валюты.
        /// </summary>
        public string Nominal { get; set; }

        /// <summary>
        /// Код родительской валюты.
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// ISO-Num-Code.
        /// </summary>
        public string IsoNumCode { get; set; }

        /// <summary>
        /// ISO-Char-Code.
        /// </summary>
        public string IsoCharCode { get; set; }
    }
}
