using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Core.BusinessLogics.Services;
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
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Сервис получений информации о валютах.
        /// </summary>
        private readonly GetCurranciesService _getCurranciesService;

        public HangfireJobController(
            ILogger<HangfireJobController> logger,
            GetCurranciesService getCurranciesService)
        {
            _logger = logger;
            _getCurranciesService = getCurranciesService;
        }

        /// <summary>
        /// Создание единичной задачи (Job) получения справочной информации о валютах.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("send-valuta-infos-only-fire-and-forget")]
        public IActionResult AddJobGetValutaInfoOnly()
        {
            _logger.LogInformation("Добавляем единичную задачу (Job) получения справочной информации о валютах.");
            BackgroundJob.Enqueue(() => _getCurranciesService.RequestCurrencyInfoOnlyAsync());
            return Ok("Задача получения справочной информации о валютах добавлена.");
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
            BackgroundJob.Enqueue(() => _getCurranciesService.RequestCurrencyInfosAsync());
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
            RecurringJob.AddOrUpdate("Daily Currencies Notification", () => _getCurranciesService.RequestCurrencyInfosAsync(), "0 0 * * *");
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
            BackgroundJob.Schedule(() => _getCurranciesService.RequestCurrencyInfoOnlyAsync(), TimeSpan.FromSeconds(1));
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
            var firstJob = BackgroundJob.Enqueue(() => _getCurranciesService.RequestCurrencyInfosAsync());
            BackgroundJob.ContinueJobWith(firstJob, () => _getCurranciesService.RequestCurrencyInfosAsync());
            return Ok("Продолжающаяся задача получения справочной информации о валютах и валютных котировках по датам добавлена.");
        }

        #endregion
    }
}
