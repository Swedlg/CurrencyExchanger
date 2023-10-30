namespace Storage.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Значение валюты к другой валюте по дате.
    /// </summary>
    [Serializable]
    public class CurrencyValueByDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID Базовой валюты.
        /// </summary>
        public int BaseCurrencyId { get; set; }

        /// <summary>
        /// ID другой валюты.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public decimal Value { get; set; }
    }
}
