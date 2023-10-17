using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Database.Models
{
    /// <summary>
    /// Значение валюты к другой валюте по дате.
    /// </summary>
    [Table("currencyvaluebydates")]
    public class CurrencyValueByDate
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
        /// Базовая валюта.
        /// </summary>
        public CurrencyInfo BaseCurrency { get; set; } // (Вторичный ключ).

        /// <summary>
        /// ID Другой валюты.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Другая валюта.
        /// </summary>
        public CurrencyInfo Currency { get; set; } // (Вторичный ключ).

        /// <summary>
        /// Дата.
        /// </summary>
        public DateOnly Date { get; set; } // DateOnly (.NET) -> date (PostgreSQL)

        /// <summary>
        /// Значение курса валют на дату.
        /// </summary>
        public decimal Value { get; set; } // decimal (.NET) -> numeric (PostgreSQL)
    }
}
