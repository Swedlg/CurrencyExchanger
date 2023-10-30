using System.Xml.Serialization;

namespace Crawler.Core.BusinessLogics.Helpers
{
    /// <summary>
    /// Вспомагательный класс для парсинга XML строки в нужный формат.
    /// </summary>
    public static class XmlParseHelper
    {
        /// <summary>
        /// Запарсить строку в список справочной информации о валютах.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Нужный формат.</returns>
        public static T? Parse<T>(string xmlString)
        {
            XmlSerializer serializer = new(typeof(T));
            T? currencyData;

            using (var reader = new StringReader(xmlString))
            {
                currencyData = (T?)serializer.Deserialize(reader);
            }

            return currencyData;
        }
    }
}
