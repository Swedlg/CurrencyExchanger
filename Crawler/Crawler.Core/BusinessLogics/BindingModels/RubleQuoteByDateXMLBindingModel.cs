using System.Xml.Serialization;

namespace Crawler.Core.BindingModels
{
    /// <summary>
    /// Модель информации о валютной котировке относительно рубля.
    /// </summary>
    public class RubleQuoteByDateXMLBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        [XmlAttribute("ID")]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Цифровой код валюты.
        /// </summary>
        [XmlElement("NumCode")]
        public string NumCode { get; set; } = String.Empty;

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        [XmlElement("CharCode")]
        public string CharCode { get; set; } = String.Empty;

        /// <summary>
        /// Номинал валюты.
        /// </summary>
        [XmlElement("Nominal")]
        public string Nominal { get; set; } = String.Empty;

        /// <summary>
        /// Название валюты на русском.
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Значение стоимости валюты.
        /// </summary>
        [XmlElement("Value")]
        public string Value { get; set; } = String.Empty;

        /// <summary>
        /// Значение стоимости валюты соотносительно с номиналом.
        /// </summary>
        [XmlElement("VunitRate")]
        public string VunitRate { get; set; } = String.Empty;
    }
}
