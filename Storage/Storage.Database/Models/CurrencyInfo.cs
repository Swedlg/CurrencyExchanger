using Storage.Core.BusinessLogics.BindingModels;

namespace Storage.Database.Models
{
    /// <summary>
    /// Справочная информация о валюте.
    /// </summary>
    public class CurrencyInfo : CurrencyInfoBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public override string EngName { get; set; }

        /// <summary>
        /// Код родительской или базовой валюты.
        /// </summary>
        public override string RId { get; set; }

        /// <summary>
        /// Код валюты.
        /// </summary>
        public override string ISOCharCode { get; set; }
    }
}
