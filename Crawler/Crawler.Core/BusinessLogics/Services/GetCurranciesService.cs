using Crawler.Core.BusinessLogics.Helpers;
using ExchangeData.DTOModels;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;
using System.Text;
using System.Text.Unicode;

namespace Crawler.Core.BusinessLogics.Services
{
    /// <summary>
    /// Сервис получения данных о валютных котировках.
    /// </summary>
    public static class GetCurranciesService
    {
        /// <summary>
        /// Дефолтный URL для запроса получения справочной информации о валютах.
        /// </summary>
        private static readonly string cbr_currency_info_url = "http://cbr.ru/scripts/XML_valFull.asp";

        /// <summary>
        /// Дефолтный URL для запроса получения данных о валютных котировках.
        /// </summary>
        private static readonly string cbr_currancy_value_by_date_url = "http://cbr.ru/scripts/XML_daily.asp?date_req="; // Например 02/03/2002

        /// <summary>
        /// Асинхронный метод получения справочной информации о валютах.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>:)</returns>
        public static async Task<CurrencyInfoListDTO> GetCurrencyInfoByURLAsync(string url = "http://cbr.ru/scripts/XML_valFull.asp")
        {
            string xmlString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    xmlString = encoding.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"Загружена справочная информация о валютах");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                    throw e;
                }    
            }
            return XmlParseHelper.ParseXmlCurrencyInfoToDTO(xmlString);
        }

        /// <summary>
        /// Асинхронный метод пролучения данных о валютных котировках по дате. Например 02/03/2022.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="date">Дата.</param>
        /// <returns>:)</returns>
        public static async Task<RubleQuotesByDateDTO> GetCurrencyByDateByURLAsync(DateOnly date, string url = "http://cbr.ru/scripts/XML_daily.asp?date_req=")
        {
            string xmlString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var bytes = await client.GetByteArrayAsync($"{url}{date.ToString("dd/MM/yyyy")}");
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    xmlString = encoding.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"Загружена информация о валютных котировках по дате: {date}");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                    throw e;
                }
            }
            return XmlParseHelper.ParseXmlCurrencyQuotesByDateToDTO(xmlString);
        }

        /// <summary>
        /// Асинхронный метод получения массива json с котировками валют по датам
        /// </summary>
        /// <param name="fromDate">Дата.</param>
        /// <param name="url_info">URl.</param>
        /// <param name="url_values">URl.</param>
        /// <returns></returns>
        public static async Task<string> GetCurrencyInfosInJsonAsync(
            DateOnly? fromDate,
            string url_info = "http://cbr.ru/scripts/XML_valFull.asp",
            string url_values = "http://cbr.ru/scripts/XML_daily.asp?date_req=")
        {
            string xmlStringInfo = String.Empty;
            List<string> xmlStringValuesList = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var bytes = await client.GetByteArrayAsync(url_info);
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    xmlStringInfo = encoding.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"Загружена справочная информация о валютах");

                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                    if (fromDate == null)
                    {
                        fromDate = currentDate.AddYears(-2);
                    }

                    DateOnly iteratorDate = (DateOnly)fromDate;
                    while (iteratorDate <= currentDate)
                    {
                        var bytesCurrencyValues = await client.GetByteArrayAsync($"{url_values}{iteratorDate.ToString("dd/MM/yyyy")}");
                        var xmlString = encoding.GetString(bytesCurrencyValues, 0, bytesCurrencyValues.Length);
                        xmlStringValuesList.Add(xmlString);
                        Console.WriteLine($"Загружена информация о валютных котировках по дате: {iteratorDate}");
                        iteratorDate = iteratorDate.AddDays(1);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                }
            }

            var currencyInfoList = XmlParseHelper.ParseXmlCurrencyInfo(xmlStringInfo);
            var currencyValuesByDetesList = XmlParseHelper.ParseXmlCurrencyValueByDate(xmlStringValuesList);

            var result = from info in currencyInfoList
                         join value in currencyValuesByDetesList
                         on info.IsoCharCode equals value.CharCode
                         select new
                         {
                             info.Name,
                             info.EngName,
                             info.IsoCharCode,
                             info.Nominal,
                             value.Date,
                             value.Value
                         };

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}
