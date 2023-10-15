using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Core.BusinessLogics.Services;
using Crawler.Database.Models;
using ExchangeData.DTOModels;
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
        private readonly ILatestUploadDateRepository _latestUploadDateRepository;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// IBus от MassTransit.
        /// </summary>
        private readonly IBus _bus;

        public HangfireJobController(
            ILogger<HangfireJobController> logger,
            ILatestUploadDateRepository latestUploadDateRepository,
            IBus bus)
        {
            _logger = logger;
            _latestUploadDateRepository = latestUploadDateRepository;
            _bus = bus;
        }

        /// <summary>
        /// Создание единичной задачи (Job) получения списка валютных котировок.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("get-currencies-fire-and-forget")]
        public IActionResult AddJobGetCurrencies()
        {
            _logger.LogInformation("Добавляем единичную задачу (Job) получения списка валютных котировок");
            BackgroundJob.Enqueue(() => RequestCurrencyInfosAsync());
            return Ok();
        }

        /// <summary>
        /// Создание повторяющейся задачи (Job) получения списка валютных котировок. Период 1 день.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("get-currencies-recurring")]
        public IActionResult AddReccuringopGetCurrencies()
        {
            _logger.LogInformation("Добавляем повторяющуюся задачу (Job) получения списка валютных котировок");
            RecurringJob.AddOrUpdate("Daily Currencies Notification", () => RequestCurrencyInfosAsync(), "0 0 * * *");
            return Ok();
        }

        /// <summary>
        /// Метод получения DTO объектов c информацией о валютах и их котировках, и их отправки через RabbitMQ + MassTransit.
        /// </summary>
        /// <returns>:)</returns>
        public async Task RequestCurrencyInfosAsync()
        {
            var currencyInfos = await GetCurranciesService.GetCurrencyInfoByURLAsync();
            var latestUploadDate = await _latestUploadDateRepository.GetUploadDateAsync();

            CurrencyValuesByDatesListDTO? currenciesByDates;
            if (latestUploadDate == null)
            {
                currenciesByDates = await GetCurranciesService.GetCurrencyByDateByURLAsync(null);
            }
            else
            {
                var alreadyDownloaded = await _latestUploadDateRepository.UploadDateExistsAsync(DateOnly.FromDateTime(DateTime.Now));
                if (alreadyDownloaded)
                {
                    return;
                }
                else
                {
                    currenciesByDates = await GetCurranciesService.GetCurrencyByDateByURLAsync(latestUploadDate.UploadDate);
                }
            }

            bool success = await _latestUploadDateRepository.CreateUploadDateAsync(new LatestUploadDate { UploadDate = DateOnly.FromDateTime(DateTime.Now) });


            if (currencyInfos != null)
            {
                await _bus.Publish(currencyInfos);
            }

        

            var x = 0;
        }

        /*
        [HttpPost]
        [Route("fire-and-forget")]
        public IActionResult FireAndForget(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"{client}, thank you for contacting us."));

            return Ok($"Job Id: {jobId}");
        }
              
        [HttpPost]
        [Route("delayed")]
        public IActionResult Delayed(string client)
        {
            string jobId = BackgroundJob.Schedule(() =>
                Console.WriteLine($"Session for client {client} has been closed."), TimeSpan.FromSeconds(60));

            return Ok($"Job Id: {jobId}");
        }
        
        [HttpPost]
        [Route("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate("Daily Notification", () =>
                Console.WriteLine($"Happy BirthDay! :)"), Cron.Daily());

            return Ok();
        }
              
        [HttpPost]
        [Route("continuations")]
        public IActionResult Continuations(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"Check balance logic for {client}"));

            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"{client}, your balance has been changed."));

            return Ok();
        }
        */
    }
}
