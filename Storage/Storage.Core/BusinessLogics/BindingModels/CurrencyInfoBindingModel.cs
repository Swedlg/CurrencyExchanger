using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Справочная информация о валюте.
    /// </summary>
    public abstract class CurrencyInfoBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public virtual string EngName { get; set; }

        /// <summary>
        /// Код родительской или базовой валюты.
        /// </summary>
        public virtual string RId { get; set; }

        /// <summary>
        /// Код валюты.
        /// </summary>
        public virtual string ISOCharCode { get; set; }
    }
}
