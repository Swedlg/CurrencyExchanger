using Microsoft.AspNetCore.Mvc;
using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;
using System.Text.Json;

namespace Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        /// <summary>
        /// Хранилище справочной информации о валютах.
        /// </summary>
        private readonly ICurrencyInfoRepository _currencyInfoRepository;

        /// <summary>
        /// Хранилище информации о валютных котировках по датам.
        /// </summary>
        private readonly ICurrencyValueByDateRepository _currencyValueByDateRepository;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<CurrencyController> _logger;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="currencyInfoRepository">Хранилище справочной информации о валютах.</param>
        /// <param name="currencyValueByDateRepository">Хранилище информации о валютных котировках по датам.</param>
        public CurrencyController(
            ILogger<CurrencyController> logger,
            ICurrencyInfoRepository currencyInfoRepository,
            ICurrencyValueByDateRepository currencyValueByDateRepository)
        {
            _logger = logger;
            _currencyInfoRepository = currencyInfoRepository;
            _currencyValueByDateRepository = currencyValueByDateRepository;
        }

        /// <summary>
        /// Получение информации валютных котировках с фильтрацией по диапозону даи и кодам валют.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet]
        [Route("get-currencies-values")]
        public async Task<IActionResult> GetCurrenciesValues(
            DateTime? dateFrom,
            DateTime? dateTo,
            string baseRId,
            string? otherRid)
        {
            _logger.LogInformation("Вызывается метод получения информации валютных котировках с фильтрацией по диапозону даи и кодам валют.");
            List<CurrencyValueByDateBindingModel> list = ( await _currencyValueByDateRepository.GetFilteredAsync(
                dateFrom.HasValue ? DateOnly.FromDateTime((DateTime)dateFrom) : null,
                dateTo.HasValue ? DateOnly.FromDateTime((DateTime)dateTo) : null,
                baseRId,
                string.IsNullOrWhiteSpace(otherRid) ? null : otherRid)).ToList();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(list, options);

            return Ok(jsonString);
        }

        /// <summary>
        /// Получение справочной информации о валютах.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet]
        [Route("get-currencies-infos")]
        public async Task<IActionResult> GetCurrenciesInfos()
        {
            _logger.LogInformation("Вызывается метод получения справочной информации о валютах.");
            var json = await _currencyInfoRepository.GetAllCurrencyInfoAsync();
            return Ok(json);
        }

        /// <summary>
        /// Добавить рубль в справочную информацию о валютах.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        [Route("create-ruble")]
        public async Task<IActionResult> CreateRuble()
        {
            _logger.LogInformation("Вызывается метод добавления рубля в справочную информацию о валютах.");
            await _currencyInfoRepository.CreateCurrencyInfoAsync(new CurrencyInfoBindingModel
            {
                Name = "Рубль",
                EngName = "Ruble",
                RId = "R00000",
                ISOCharCode = "RUB"
            });
            return Ok("Рубль добавлен в справочную информацию о валютах.");
        }

        /// <summary>
        /// Очистка хранилища справочной информации о валютах.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpDelete]
        [Route("truncate-currency-infos-db")]
        public async Task<IActionResult> TruncateCurrencyInfoRepository()
        {
            _logger.LogInformation("Вызывается метод очистки хранилища справочной информации о валютах.");
            bool truncated = await _currencyInfoRepository.Truncate();
            if (truncated)
            {
                _logger.LogInformation("Хранилище справочной информации о валютах очищено.");
            }
            return Ok("Хранилище справочной информации о валютах очищено.");
        }

        /// <summary>
        /// Очистка хранилища информации о валютных котировках по датам.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpDelete]
        [Route("truncate-currency-values-db")]
        public async Task<IActionResult> TruncateCurrencyValueByDateRepository()
        {
            _logger.LogInformation("Вызывается метод очистки хранилища информации о валютных котировках по датам.");
            bool truncated = await _currencyValueByDateRepository.Truncate();
            if (truncated)
            {
                _logger.LogInformation("Хранилище информации о валютных котировка по датам очищено.");
            }
            return Ok("Хранилище информации о валютных котировка по датам очищено.");
        }
    }
}
