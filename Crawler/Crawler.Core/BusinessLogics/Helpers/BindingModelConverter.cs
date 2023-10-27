using Crawler.Core.BindingModels;
using Crawler.Core.BusinessLogics.BindingModels;
using Newtonsoft.Json;

namespace Crawler.Core.BusinessLogics.Helpers
{
    /// <summary>
    /// Конвертер  BindingModel'ей в различные форматы.
    /// </summary>
    public class BindingModelConverter
    {
        /// <summary>
        /// Запарсить BindingModel'и со справочной информацией о валютах и 
        /// информацию о котировках валют по датам в строку Json.
        /// </summary>
        /// <param name="valutaListXMLBindingModel"></param>
        /// <param name="valCursListXMLBindingModels"></param>
        /// <returns>Строка Json представляющая выборку данных о котировках валют по датам.</returns>
        internal static string ParseCurrencyInfoToJson(ValutaListXMLBindingModel? valutaListXMLBindingModel, List<ValCursListXMLBindingModel> valCursListXMLBindingModels)
        {
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
