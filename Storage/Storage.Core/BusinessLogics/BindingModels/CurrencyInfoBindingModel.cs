namespace Storage.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Справочная информация о валюте.
    /// </summary>
    public class CurrencyInfoBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название валюты на английском.
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// Код родительской или базовой валюты.
        /// </summary>
        public string RId { get; set; }

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        public string ISOCharCode { get; set; }
    }
}
