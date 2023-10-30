using AutoMapper;
using Crawler.Core.BusinessLogics.BindingModels;
using Crawler.Core.BusinessLogics.ConfigModels;
using Crawler.Core.BusinessLogics.Helpers;
using Crawler.Core.BusinessLogics.Interfaces;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace Crawler.Core.BusinessLogics.Services
{
    /// <summary>
    /// Сервис получения данных о валютных котировках.
    /// </summary>
    public class GetCurranciesService
    {
        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper;

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
        /// Пользовательские настройки.
        /// </summary>
        private readonly MySettings _settings;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="publishEndpoint"></param>
        /// <param name="logger"></param>
        /// <param name="latestUploadDateRepository"></param>
        /// <param name="httpClient"></param>
        /// <param name="settings"></param>
        public GetCurranciesService(
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            ILogger<GetCurranciesService> logger,
            IUploadDateRepository latestUploadDateRepository,
            HttpClient httpClient,
            IOptionsSnapshot<MySettings> settings)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _latestUploadDateRepository = latestUploadDateRepository;
            _httpClient = httpClient;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = settings.Value;
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
     
            UploadDateBindingModel? latestUploadDate = await _latestUploadDateRepository.GetUploadDateAsync();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            //DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddYears(-2);  : latestUploadDate.UploadDate;
            DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddDays(_settings.SinceDaysCount) : latestUploadDate.UploadDate;

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

            return ConvertXmlCurrencyInfoToDTO(xmlString);
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
            return ConvertXmlCurrencyQuotesByDateToDTO(xmlString);
        }

        /// <summary>
        /// Асинхронный метод получения массива json с котировками валют по датам
        /// </summary>
        /// <param name="exactlyDate">Дата.</param>
        /// <param name="url_info">URl.</param>
        /// <param name="url_values">URl.</param>
        /// <returns></returns>
        public async Task<string> GetCurrencyInfosInJsonAsync(
            DateOnly? exactlyDate,
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
                if (exactlyDate == null)
                {
                    exactlyDate = currentDate.AddYears(-2);
                }

                DateOnly iteratorDate = (DateOnly)exactlyDate;
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

            #region Парсим строки XML в BindingModel'и

            ValutaListXMLBindingModel? valutaListXMLBindingModel = XmlParseHelper.Parse<ValutaListXMLBindingModel>(xmlStringInfo);

            List<ValCursListXMLBindingModel> valCursListXMLBindingModels = new();

            foreach (var str in xmlStringValuesList)
            {
                var newListValue = XmlParseHelper.Parse<ValCursListXMLBindingModel>(str);
                if (newListValue != null)
                {
                    valCursListXMLBindingModels.Add(newListValue);
                }
            }

            #endregion

            #region Парсим BindingModel'и в нужный Json формат

            var result = BindingModelConverter.ParseCurrencyInfoToJson(valutaListXMLBindingModel, valCursListXMLBindingModels);

            #endregion

            return result;
        }

        /// <summary>
        /// Асинхронный метод получения массива json с котировками валют по определенной дате
        /// </summary>
        /// <param name="exactlyDate">Дата.</param>
        /// <param name="url_info">URl.</param>
        /// <param name="url_values">URl.</param>
        /// <returns></returns>
        public async Task<string> GetCurrencyValueByExactlyDateInJsonAsync(
            DateOnly exactlyDate,
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

                var bytesCurrencyValues = await _httpClient.GetByteArrayAsync($"{url_values}{exactlyDate:dd/MM/yyyy}");
                var xmlString = encoding.GetString(bytesCurrencyValues, 0, bytesCurrencyValues.Length);
                xmlStringValuesList.Add(xmlString);
                Console.WriteLine($"Загружена информация о валютных котировках по дате: {exactlyDate}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {e.Message}");
            }

            #region Парсим строки XML в BindingModel'и

            ValutaListXMLBindingModel? valutaListXMLBindingModel = XmlParseHelper.Parse<ValutaListXMLBindingModel>(xmlStringInfo);

            List<ValCursListXMLBindingModel> valCursListXMLBindingModels = new();

            foreach (var str in xmlStringValuesList)
            {
                var newListValue = XmlParseHelper.Parse<ValCursListXMLBindingModel>(str);
                if (newListValue != null)
                {
                    valCursListXMLBindingModels.Add(newListValue);
                }
            }

            #endregion

            #region Парсим BindingModel'и в нужный Json формат

            var result = BindingModelConverter.ParseCurrencyInfoToJson(valutaListXMLBindingModel, valCursListXMLBindingModels);

            #endregion

            return result;
        }

        /// <summary>
        /// Запарсить строку в список справочной информации о валютах.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Объект CurrencyInfoListDTO.</returns>
        public CurrencyInfoListDTO ConvertXmlCurrencyInfoToDTO(string xmlString)
        {
            List<CurrencyInfoDTO> itemList = new()
            {
                new CurrencyInfoDTO()
                {
                    RId = "R00000", // ( Я не знаю настоящий RId для рубля )
                    Name = "Рубль",
                    EngName = "Ruble",
                    ParentRId = "R00000",
                    ISOCharCode = "RUB"
                }
            };

            ValutaListXMLBindingModel? currencyData = XmlParseHelper.Parse<ValutaListXMLBindingModel>(xmlString);

            List<CurrencyInfoDTO>? valuteInfoList = currencyData?.Items?.Select(valuteInfo => _mapper.Map<CurrencyInfoDTO>(valuteInfo)).ToList();

            if (valuteInfoList != null)
            {
                itemList.AddRange(valuteInfoList);
            }

            return new CurrencyInfoListDTO() { List = itemList ?? new List<CurrencyInfoDTO>() };
        }


        /// <summary>
        /// Запарсить строку в список информации о валютных котировках по датам.
        /// </summary>
        /// <param name="xmlString">XML строка.</param>
        /// <returns>Объект CurrencyValueByDateListDTO.</returns>
        public RubleQuotesByDateDTO ConvertXmlCurrencyQuotesByDateToDTO(string xmlString)
        {
            List<RubleQuoteDTO> itemList = new();

            ValCursListXMLBindingModel? currencyData = XmlParseHelper.Parse<ValCursListXMLBindingModel>(xmlString);

            List<RubleQuoteDTO>? currencyValuesList = currencyData?.Valutes?.Select(valuteValue => _mapper.Map<RubleQuoteDTO>(valuteValue)).ToList();

            if (currencyValuesList != null)
            {
                foreach (var valuteValue in currencyValuesList)
                {
                    valuteValue.BaseCurrencyId = "R00000";
                }

                itemList.AddRange(currencyValuesList);
            }

            return new RubleQuotesByDateDTO
            {
                Date = DateTime.ParseExact(currencyData?.Date ?? "01.01.0001", "dd.MM.yyyy", null),
                List = itemList
            };
        }
    }
}
