using System.Xml.Serialization;

namespace Crawler.Core.BindingModels
{
    /// <summary>
    /// Модель информации о валюте.
    /// </summary>
    public class CurrencyInfoXMLBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        [XmlAttribute("ID")]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты на английском.
        /// </summary>
        [XmlElement("EngName")]
        public string EngName { get; set; } = String.Empty;

        /// <summary>
        /// Номинал валюты.
        /// </summary>
        [XmlElement("Nominal")]
        public string Nominal { get; set; } = String.Empty;

        /// <summary>
        /// Код родительской валюты.
        /// </summary>
        [XmlElement("ParentCode")]
        public string ParentCode { get; set; } = String.Empty;

        /// <summary>
        /// ISO-Num-Code.
        /// </summary>
        [XmlElement("ISO_Num_Code")]
        public string IsoNumCode { get; set; } = String.Empty;

        /// <summary>
        /// ISO-Char-Code.
        /// </summary>
        [XmlElement("ISO_Char_Code")]
        public string IsoCharCode { get; set; } = String.Empty;
    }
}
