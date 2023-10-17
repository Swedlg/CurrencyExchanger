using Crawler.Core.BusinessLogics.BindingModels;
using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Core.BusinessLogics.Services;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Main.Controllers
{
    /// <summary>
    /// Контроллер для создания задач (Jobs) с помощью Hangfire.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireJobController : ControllerBase
    {
        /// <summary>
        /// Хранилище дат последних загрузок.
        /// </summary>
        private readonly IUploadDateRepository _latestUploadDateRepository;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Опредедитель Endpoint'ов для MassTransit и RabbitMQ
        /// </summary>
        public readonly IPublishEndpoint _publishEndpoint;

        public HangfireJobController(
            ILogger<HangfireJobController> logger,
            IUploadDateRepository latestUploadDateRepository,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _latestUploadDateRepository = latestUploadDateRepository;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Создание единичной задачи (Job) получения справочной информации о валютах
        /// и валютных котировках по датам.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("get-currencies-fire-and-forget")]
        public IActionResult AddJobGetCurrencies()
        {
            _logger.LogInformation("Добавляем единичную задачу (Job) получения справочной информации о валютах и валютных котировках по датам.");
            BackgroundJob.Enqueue(() => RequestCurrencyInfosAsync());
            return Ok("Задача получения справочной информации о валютах и валютных котировках по датам добавлена.");
        }

        /// <summary>
        /// Создание повторяющейся задачи (Job) получения справочной информации о валютах
        /// и валютных котировках по датам. Период 1 день.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("get-currencies-recurring")]
        public IActionResult AddJobReccuringGetCurrencies()
        {
            _logger.LogInformation("Добавляем повторяющуюся задачу (Job) получения справочной информации о валютах и валютных котировках по датам.");
            RecurringJob.AddOrUpdate("Daily Currencies Notification", () => RequestCurrencyInfosAsync(), "0 0 * * *");
            return Ok("Повторяющаяся задача получения справочной информации о валютах и валютных котировках по датам добавлена.");
        }

        #region Памятка по использованию отложенных и продолжающихся задач

        /// <summary>
        /// Создание отложенной задачи (Job) получения получения справочной информации о валютах
        /// и валютных котировках по датам. (Отложена на 1 секунду).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-currencies-info-delayed")]
        public IActionResult AddJobDelaydGetCurrencies()
        {
            _logger.LogInformation("Добавляем отложенную задачу (Job) получения справочной информации о валютах и валютных котировках по датам.");
            BackgroundJob.Schedule(() => RequestCurrencyInfoOnlyAsync(), TimeSpan.FromSeconds(1));
            return Ok("Отложенная задача получения справочной информации о валютах и валютных котировках по датам добавлена.");
        }

        /// <summary>
        /// Создание продолжающейся задачи (Job) получения получения справочной информации о валютах
        /// и валютных котировках по датам.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("get-currencies-continuations")]
        public IActionResult AddJobContinuationsGetCurrencies()
        {
            _logger.LogInformation("Добавляем продолжающуюся задачу (Job) получения справочной информации о валютах и валютных котировках по датам.");
            var firstJob = BackgroundJob.Enqueue(() => RequestCurrencyInfosAsync());
            BackgroundJob.ContinueJobWith(firstJob, () => RequestCurrencyInfosAsync());
            return Ok("Продолжающаяся задача получения справочной информации о валютах и валютных котировках по датам добавлена.");
        }

        #endregion

        /// <summary>
        /// Метод получения DTO объектов c информацией о валютах и их котировках, и их отправки через RabbitMQ + MassTransit.
        /// </summary>
        /// <returns>:)</returns>
        public async Task RequestCurrencyInfosAsync()
        {
            #region Получение справочной информации о валютах и отправка в Storage

            CurrencyInfoListDTO currencyInfoListDTO = await GetCurranciesService.GetCurrencyInfoByURLAsync();

            if (currencyInfoListDTO != null)
            {
                _logger.LogInformation("Отправляем справочную информацию о валютах");
                await _publishEndpoint.Publish(currencyInfoListDTO);
                _logger.LogInformation("Cправочную информацию о валютах отправлена");
            }

            #endregion

            #region Получение информации о котировках валют относительно рубля и отправка соответсвующего DTO в Converter

            UploadDateBindingModel latestUploadDate = await _latestUploadDateRepository.GetUploadDateAsync();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            //DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddYears(-2);  : latestUploadDate.UploadDate;
            DateOnly iteratorDate = latestUploadDate == null ? currentDate.AddMonths(-1) : latestUploadDate.UploadDate;

            if (iteratorDate < currentDate)
            {
                while (iteratorDate <= currentDate)
                {
                    RubleQuotesByDateDTO rubleQuotesByDateDTO = await GetCurranciesService.GetCurrencyByDateByURLAsync(iteratorDate);

                    if (rubleQuotesByDateDTO != null)
                    {
                        _logger.LogInformation($"Отправляем валютные котировке по дате {iteratorDate}.");
                        await _publishEndpoint.Publish(rubleQuotesByDateDTO);
                        _logger.LogInformation($"Валютные котировки по дате {iteratorDate} отправлены.");
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
            CurrencyInfoListDTO currencyInfoListDTO = await GetCurranciesService.GetCurrencyInfoByURLAsync();

            if (currencyInfoListDTO != null)
            {
                _logger.LogInformation("Отправляем справочную информацию о валютах");
                await _publishEndpoint.Publish(currencyInfoListDTO);
                _logger.LogInformation("Cправочную информацию о валютах отправлена");
            }
        }
    }
}
