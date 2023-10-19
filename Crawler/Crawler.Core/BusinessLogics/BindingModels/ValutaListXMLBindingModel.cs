using Crawler.Core.BindingModels;
using System.Xml.Serialization;

namespace Crawler.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Модель списка информаций о валютах.
    /// </summary>
    [XmlType("Valuta")]
    public class ValutaListXMLBindingModel
    {
        /// <summary>
        /// Список информаций о валютах.
        /// </summary>
        [XmlElement("Item")]
        public List<CurrencyInfoXMLBindingModel>? Items { get; set; }
    }
}
