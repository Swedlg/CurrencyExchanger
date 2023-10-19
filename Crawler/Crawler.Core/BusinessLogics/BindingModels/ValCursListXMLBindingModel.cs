using Crawler.Core.BindingModels;
using System.Xml.Serialization;

namespace Crawler.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Модель списка информации о валютных котировках.
    /// </summary>
    [XmlType("ValCurs")]
    public class ValCursListXMLBindingModel
    {
        /// <summary>
        /// Дата.
        /// </summary>
        [XmlAttribute("Date")]
        public string Date { get; set; } = String.Empty;

        /// <summary>
        /// Список информаций о валютных котировках.
        /// </summary>
        [XmlElement("Valute")]
        public List<RubleQuoteByDateXMLBindingModel> Valutes { get; set; } = new List<RubleQuoteByDateXMLBindingModel>();
    }
}
