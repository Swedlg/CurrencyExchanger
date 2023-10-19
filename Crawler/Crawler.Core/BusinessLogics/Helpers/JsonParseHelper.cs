using Crawler.Core.BindingModels;
using Crawler.Core.BusinessLogics.BindingModels;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Crawler.Core.BusinessLogics.Helpers
{
    /// <summary>
    /// Парсер Json.
    /// </summary>
    public class JsonParseHelper
    {
        /// <summary>
        /// Запарсить XML строки справочной информации о валютах и информации о котировках валют в выборку в формате Json.
        /// </summary>
        /// <param name="xmlStringInfo">XML строка информаций о валютах.</param>
        /// <param name="xmlStringValuesList">Список XML строк информации о котировках валют.</param>
        /// <returns>Строка Json представляющая выборку данных о котировках валют по датам.</returns>
        internal static string ParseCurrencyInfoToJson(string xmlStringInfo, List<string> xmlStringValuesList)
        {
            XmlSerializer serializer = new(typeof(ValutaListXMLBindingModel));
            ValutaListXMLBindingModel? valutaListXMLBindingModel;

            using (var reader = new StringReader(xmlStringInfo))
            {
                valutaListXMLBindingModel = (ValutaListXMLBindingModel?)serializer.Deserialize(reader);
            }

            serializer = new XmlSerializer(typeof(ValCursListXMLBindingModel));
            List<ValCursListXMLBindingModel> valCursListXMLBindingModels = new();

            foreach (var str in xmlStringValuesList)
            {
                using var reader = new StringReader(str);
                var currencyValueData = (ValCursListXMLBindingModel?)serializer.Deserialize(reader);
                if (currencyValueData != null)
                {
                    valCursListXMLBindingModels.Add(currencyValueData);
                }
            }

            List<JsonCurrencyByDateDTO> jsonCurrencyByDateDTOs = new();

            foreach (var dateValues in valCursListXMLBindingModels)
            {
                DateTime dateTime = DateTime.ParseExact(dateValues.Date, "dd.MM.yyyy", null);

                foreach (var currencyValue in dateValues.Valutes)
                {
                    CurrencyInfoXMLBindingModel? curInfo = valutaListXMLBindingModel?.Items?
                        .Where(v => v.IsoCharCode == currencyValue.CharCode)
                        .FirstOrDefault();

                    jsonCurrencyByDateDTOs.Add(new JsonCurrencyByDateDTO()
                    {
                        Name = currencyValue.Name,
                        EngName = curInfo == null ? "" : curInfo.Name,
                        IsoCharCode = currencyValue.CharCode,
                        Nominal = Convert.ToDecimal(currencyValue.Nominal),
                        Date = dateTime,
                        Value = Convert.ToDecimal(currencyValue.Value),
                    });
                }
            }

            return JsonConvert.SerializeObject(jsonCurrencyByDateDTOs, Formatting.Indented);
        }
    }
}
