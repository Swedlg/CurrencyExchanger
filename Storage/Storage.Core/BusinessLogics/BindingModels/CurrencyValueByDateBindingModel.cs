namespace Storage.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Значение валюты по дате.
    /// </summary>
    public abstract class CurrencyValueByDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// ID Базовой валюты.
        /// </summary>
        public virtual int BaseCurrencyId { get; set; }

        /// <summary>
        /// ID Валюты.
        /// </summary>
        public virtual int CurrencyId { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public virtual DateOnly Date { get; set; }

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public virtual decimal Value { get; set; }
    }
}
