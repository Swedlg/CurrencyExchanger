using Crawler.Core.BusinessLogics.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Main.Controllers
{
    /// <summary>
    /// Контроллер для получения информации о валютах
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        /// <summary>
        /// Сервис получений информации о валютах.
        /// </summary>
        private readonly GetCurranciesService _getCurranciesService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="getCurranciesService">Сервис получения информации о валютах.</param>
        public CurrencyController(GetCurranciesService getCurranciesService)
        {
            _getCurranciesService = getCurranciesService;
        }

        /// <summary>
        /// Получение информации о валютных котировках в формате json
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet]
        [Route("get-currencies")]
        public async Task<IActionResult> GetCurrenciesInJsonArray()
        {
            var json = await _getCurranciesService.GetCurrencyInfosInJsonAsync(null);
            return Ok(json);
        }

        /// <summary>
        /// Получение информации о валютных котировках в формате json
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet]
        [Route("get-currencies-by-date")]
        public async Task<IActionResult> GetCurrenciesInJsonArray(DateTime date)
        {
            var json = await _getCurranciesService.GetCurrencyInfosInJsonAsync(DateOnly.FromDateTime(date));
            return Ok(json);
        }
    }
}
