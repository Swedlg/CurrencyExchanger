using Storage.Core.BusinessLogics.BindingModels;

namespace Storage.Database.Models
{
    /// <summary>
    /// Значение валюты по дате.
    /// </summary>
    public class CurrencyValueByDate : CurrencyValueByDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// ID Базовой валюты.
        /// </summary>
        public override int BaseCurrencyId { get; set; }

        /// <summary>
        /// Базовая валюта.
        /// </summary>
        public CurrencyInfo BaseCurrency { get; set; } // (Вторичный ключ).

        /// <summary>
        /// ID Валюты.
        /// </summary>
        public override int CurrencyId { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public CurrencyInfo Currency { get; set; } // (Вторичный ключ).

        /// <summary>
        /// Дата.
        /// </summary>
        public override DateOnly Date { get; set; } // DateOnly (.NET) -> date (PostgreSQL)

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public override decimal Value { get; set; } // decimal (.NET) -> numeric (PostgreSQL)
    }
}
