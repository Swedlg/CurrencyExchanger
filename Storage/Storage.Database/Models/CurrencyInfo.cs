using Storage.Core.BusinessLogics.BindingModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Database.Models
{
    /// <summary>
    /// Справочная информация о валюте.
    /// </summary>
    [Table("currencyinfos")]
    public class CurrencyInfo
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public string EngName { get; set; } = String.Empty;

        /// <summary>
        /// Код родительской или базовой валюты.
        /// </summary>
        public string RId { get; set; } = String.Empty;

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        public string ISOCharCode { get; set; } = String.Empty;
    }
}
