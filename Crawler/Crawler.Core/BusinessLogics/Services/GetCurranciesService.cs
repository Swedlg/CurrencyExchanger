using Crawler.Core.BusinessLogics.Helpers;
using ExchangeData.DTOModels;
using Newtonsoft.Json;

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
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    xmlString = await response.Content.ReadAsStringAsync();
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
        /// Асинхронный метод пролучения данных о валютных котировках по дате.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="fromDate">Запросить до определенной даты.</param>
        /// <returns>:)</returns>
        public static async Task<CurrencyValuesByDatesListDTO> GetCurrencyByDateByURLAsync(DateOnly? fromDate, string url = "http://cbr.ru/scripts/XML_daily.asp?date_req=")
        {    
            List<string> xmlStringList = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                    if (fromDate == null)
                    {
                        fromDate = currentDate.AddYears(-2);         
                    }

                    DateOnly iteratorDate = (DateOnly) fromDate;
                    while (iteratorDate <= currentDate)
                    {
                        HttpResponseMessage response = await client.GetAsync($"{url}{iteratorDate.ToString("dd/MM/yyyy")}");
                        response.EnsureSuccessStatusCode();
                        string xmlString = await response.Content.ReadAsStringAsync();
                        xmlStringList.Add(xmlString);
                        Console.WriteLine($"Загружена информация о валютных котировках по дате: {iteratorDate}");
                        iteratorDate = iteratorDate.AddDays(1);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}"); 
                }
            }

            List<CurrencyValueByDateListDTO> currencyValueByDateListDTOs = new List<CurrencyValueByDateListDTO>();

            foreach (var xmlString in xmlStringList)
            {
                currencyValueByDateListDTOs.Add(XmlParseHelper.ParseXmlCurrencyValueByDateToDTO(xmlString));
            }

            return new CurrencyValuesByDatesListDTO(currencyValueByDateListDTOs);
        }


        /// <summary>
        /// Асинхронный метод получения массива json с котировками валют по датам
        /// </summary>
        /// <param name="fromDate">Дата.</param>
        /// <param name="url_info">URl.</param>
        /// <param name="url_values">URl.</param>
        /// <returns></returns>
        public static async Task<string> GetCurrencyInfosInJsobAsync(
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
                    HttpResponseMessage response = await client.GetAsync(url_info);
                    response.EnsureSuccessStatusCode();
                    xmlStringInfo = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Загружена справочная информация о валютах");

                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                    if (fromDate == null)
                    {
                        fromDate = currentDate.AddYears(-2);
                    }

                    DateOnly iteratorDate = (DateOnly)fromDate;
                    while (iteratorDate <= currentDate)
                    {
                        response = await client.GetAsync($"{url_values}{iteratorDate.ToString("dd/MM/yyyy")}");
                        response.EnsureSuccessStatusCode();
                        string xmlString = await response.Content.ReadAsStringAsync();
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
