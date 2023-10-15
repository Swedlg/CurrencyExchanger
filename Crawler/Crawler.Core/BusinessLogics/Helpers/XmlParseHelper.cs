using Crawler.Core.BindingModels;
using ExchangeData.DTOModels;
using System.Xml;

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
        /// <returns>Объект CurrencyInfoListDTO.</returns>
        public static CurrencyInfoListDTO ParseXmlCurrencyInfoToDTO(string xmlString)
        {
            List<CurrencyInfoDTO> itemList = new List<CurrencyInfoDTO>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            XmlNodeList itemNodes = xmlDoc.GetElementsByTagName("Item");

            foreach (XmlNode itemNode in itemNodes)
            {
                CurrencyInfoDTO item = new CurrencyInfoDTO(
                    itemNode.Attributes["ID"].Value,
                    itemNode.SelectSingleNode("Name").InnerText,
                    itemNode.SelectSingleNode("EngName").InnerText,
                    itemNode.SelectSingleNode("ParentCode").InnerText,
                    itemNode.SelectSingleNode("ISO_Char_Code").InnerText);
                itemList.Add(item);
            }

            return new CurrencyInfoListDTO(itemList);
        }

        /// <summary>
        /// Запарсить строку в список информации о валютных котировках по датам.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Объект CurrencyValueByDateListDTO.</returns>
        public static CurrencyValueByDateListDTO ParseXmlCurrencyValueByDateToDTO(string xmlString)
        {
            List<CurrencyValueByDateDTO> itemList = new List<CurrencyValueByDateDTO>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            DateOnly date = DateOnly.Parse(xmlDoc.SelectSingleNode("ValCurs").Attributes["Date"].Value);
            XmlNodeList itemNodes = xmlDoc.GetElementsByTagName("Valute");

            foreach (XmlNode itemNode in itemNodes)
            {
                CurrencyValueByDateDTO item = new CurrencyValueByDateDTO(
                    "R00000",
                    itemNode.Attributes["ID"].Value,
                    Convert.ToDecimal(itemNode.SelectSingleNode("VunitRate").InnerText));
                itemList.Add(item);
            }

            return new CurrencyValueByDateListDTO(date, itemList);
        }

        /// <summary>
        /// Запарсить строку в список справочной информации о валютах.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Список справочной информации о валютах.</returns>
        public static List<CurrencyInfoBindingModel> ParseXmlCurrencyInfo(string xmlString)
        {
            List<CurrencyInfoBindingModel> itemList = new List<CurrencyInfoBindingModel>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            XmlNodeList itemNodes = xmlDoc.GetElementsByTagName("Item");

            foreach (XmlNode itemNode in itemNodes)
            {
                CurrencyInfoBindingModel item = new CurrencyInfoBindingModel
                {
                    Id = itemNode.Attributes["ID"].Value,
                    Name = itemNode.SelectSingleNode("Name").InnerText,
                    EngName = itemNode.SelectSingleNode("EngName").InnerText,
                    Nominal = itemNode.SelectSingleNode("Nominal").InnerText,
                    ParentCode = itemNode.SelectSingleNode("ParentCode").InnerText,
                    IsoNumCode = itemNode.SelectSingleNode("ISO_Num_Code").InnerText,
                    IsoCharCode = itemNode.SelectSingleNode("ISO_Char_Code").InnerText,
                };
                itemList.Add(item);
            }
            return itemList;
        }

        /// <summary>
        /// Запарсить строку в список информации о валютных котировках по датам.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Список информации о валютных котировках по датам.</returns>
        public static List<CurrencyValueByDateBindingModel> ParseXmlCurrencyValueByDate(List<string> xmlStrings)
        {
            List<CurrencyValueByDateBindingModel> itemList = new List<CurrencyValueByDateBindingModel>();

            XmlDocument xmlDoc = new XmlDocument();

            foreach (var xmlString in xmlStrings)
            {        
                xmlDoc.LoadXml(xmlString);
                DateOnly date = DateOnly.Parse(xmlDoc.SelectSingleNode("ValCurs").Attributes["Date"].Value);
                XmlNodeList itemNodes = xmlDoc.GetElementsByTagName("Valute");

                foreach (XmlNode itemNode in itemNodes)
                {
                    CurrencyValueByDateBindingModel item = new CurrencyValueByDateBindingModel
                    {
                        Id = itemNode.Attributes["ID"].Value,
                        Date = date,
                        NumCode = itemNode.SelectSingleNode("NumCode").InnerText,
                        CharCode = itemNode.SelectSingleNode("CharCode").InnerText,
                        Nominal = itemNode.SelectSingleNode("Nominal").InnerText,
                        Name = itemNode.SelectSingleNode("Name").InnerText,
                        Value = itemNode.SelectSingleNode("Value").InnerText,
                        VunitRate = itemNode.SelectSingleNode("VunitRate").InnerText
                    };
                    itemList.Add(item);
                }  
            }
            return itemList;
        }
    }
}
