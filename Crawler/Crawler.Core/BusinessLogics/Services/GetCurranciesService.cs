using Crawler.Core.BusinessLogics.BindingModels;
using Crawler.Core.BusinessLogics.Helpers;
using Crawler.Core.BusinessLogics.Interfaces;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Crawler.Core.BusinessLogics.Services
{
    /// <summary>
    /// Сервис получения данных о валютных котировках.
    /// </summary>
    public class GetCurranciesService
    {
        /// <summary>
        /// Парсер XML.
        /// </summary>
        private readonly XmlParseHelper _xmlParseHelper;

        /// <summary>
        /// Опредедитель Endpoint'ов для MassTransit и RabbitMQ
        /// </summary>
        public readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<GetCurranciesService> _logger;

        /// <summary>
        /// Хранилище дат последних загрузок.
        /// </summary>
        private readonly IUploadDateRepository _latestUploadDateRepository;

        /// <summary>
        /// HttpClient.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlParseHelper"></param>
        /// <param name="publishEndpoint"></param>
        /// <param name="logger"></param>
        /// <param name="latestUploadDateRepository"></param>
        /// <param name="httpClient"></param>
        public GetCurranciesService(
            XmlParseHelper xmlParseHelper,
            IPublishEndpoint publishEndpoint,
            ILogger<GetCurranciesService> logger,
            IUploadDateRepository latestUploadDateRepository,
            HttpClient httpClient)
        {
            _xmlParseHelper = xmlParseHelper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _latestUploadDateRepository = latestUploadDateRepository;
            _httpClient = httpClient;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public GetCurranciesService()
        {
        }

        /// <summary>
        /// Метод получения DTO объектов c информацией о валютах и их котировках, и их отправки через RabbitMQ + MassTransit.
        /// </summary>
        /// <returns>:)</returns>
        public async Task RequestCurrencyInfosAsync()
        {
            #region Получение справочной информации о валютах и отправка в Storage

            CurrencyInfoListDTO currencyInfoListDTO = await GetCurrencyInfoByURLAsync();

            if (currencyInfoListDTO != null)
            {
                _logger.LogInformation("Отправляем справочную информацию о валютах");
                await _publishEndpoint.Publish(currencyInfoListDTO);
                _logger.LogInformation("Cправочную информацию о валютах отправлена");
            }

            #endregion

            #region Получение информации о котировках валют относительно рубля и отправка соответсвующего DTO в Converter

            UploadDateBindingModel? latestUploadDate = await _latestUploadDateRepository.GetUploadDateAsync();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            //DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddYears(-2);  : latestUploadDate.UploadDate;
            DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddMonths(-1) : latestUploadDate.UploadDate;

            if (iteratorDate < currentDate)
            {
                while (iteratorDate <= currentDate)
                {
                    RubleQuotesByDateDTO rubleQuotesByDateDTO = await GetCurrencyByDateByURLAsync(iteratorDate);

                    if (rubleQuotesByDateDTO != null)
                    {
                        _logger.LogInformation("Отправляем валютные котировке по дате {iteratorDate}.", iteratorDate);
                        await _publishEndpoint.Publish(rubleQuotesByDateDTO);
                        _logger.LogInformation("Валютные котировки по дате {iteratorDate} отправлены.", iteratorDate);
                    }
                    iteratorDate = iteratorDate.AddDays(1);
                }

                bool success = await _latestUploadDateRepository
                    .CreateUploadDateAsync(new UploadDateBindingModel
                    {
                        UploadDate = DateOnly.FromDateTime(DateTime.Now)
                    });

                if (success)
                {
                    _logger.LogInformation("Все данные по котировкам валют загружены и отправлены в Converter.");
                }
            }
            else
            {
                _logger.LogInformation("Данные по котировкам валют уже были загружены сегодня.");
            }

            #endregion
        }

        /// <summary>
        /// Метод получения DTO объектов c информацией о валютах и их отправки через RabbitMQ + MassTransit.
        /// </summary>
        /// <returns>:)</returns>
        public async Task RequestCurrencyInfoOnlyAsync()
        {
            CurrencyInfoListDTO currencyInfoListDTO = await GetCurrencyInfoByURLAsync();

            if (currencyInfoListDTO != null)
            {
                _logger.LogInformation("Отправляем справочную информацию о валютах");
                await _publishEndpoint.Publish(currencyInfoListDTO);
                _logger.LogInformation("Cправочную информацию о валютах отправлена");
            }
        }

        /// <summary>
        /// Асинхронный метод получения справочной информации о валютах.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>:)</returns>
        private async Task<CurrencyInfoListDTO> GetCurrencyInfoByURLAsync(string url = "http://cbr.ru/scripts/XML_valFull.asp")
        {
            string xmlString;
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(url);
                xmlString = Encoding.GetEncoding("windows-1251").GetString(bytes, 0, bytes.Length);
                Console.WriteLine($"Загружена справочная информация о валютах");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                throw e;
            }

            return _xmlParseHelper.ParseXmlCurrencyInfoToDTO(xmlString);
        }

        /// <summary>
        /// Асинхронный метод пролучения данных о валютных котировках по дате. Например 02/03/2022.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="date">Дата.</param>
        /// <returns>:)</returns>
        private async Task<RubleQuotesByDateDTO> GetCurrencyByDateByURLAsync(DateOnly date, string url = "http://cbr.ru/scripts/XML_daily.asp?date_req=")
        {
            string xmlString;
            try
            {
                var bytes = await _httpClient.GetByteArrayAsync($"{url}{date:dd/MM/yyyy}");
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                xmlString = encoding.GetString(bytes, 0, bytes.Length);
                Console.WriteLine($"Загружена информация о валютных котировках по дате: {date}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
                throw e;
            }
            return _xmlParseHelper.ParseXmlCurrencyQuotesByDateToDTO(xmlString);
        }

        /// <summary>
        /// Асинхронный метод получения массива json с котировками валют по датам
        /// </summary>
        /// <param name="fromDate">Дата.</param>
        /// <param name="url_info">URl.</param>
        /// <param name="url_values">URl.</param>
        /// <returns></returns>
        public async Task<string> GetCurrencyInfosInJsonAsync(
            DateOnly? fromDate,
            string url_info = "http://cbr.ru/scripts/XML_valFull.asp",
            string url_values = "http://cbr.ru/scripts/XML_daily.asp?date_req=")
        {
            string xmlStringInfo = String.Empty;
            List<string> xmlStringValuesList = new();

            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(url_info);
                Encoding encoding = Encoding.GetEncoding("windows-1251");
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
                    var bytesCurrencyValues = await _httpClient.GetByteArrayAsync($"{url_values}{iteratorDate:dd/MM/yyyy}");
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

            return JsonParseHelper.ParseCurrencyInfoToJson(xmlStringInfo, xmlStringValuesList);
        }
    }
}
