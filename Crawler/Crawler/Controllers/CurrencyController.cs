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
        /// Получение информации о валютных котировках в формате json
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet]
        [Route("get-currencies")]
        public async Task<IActionResult> GetCurrenciesInJsonArray()
        {
            var json = await GetCurranciesService.GetCurrencyInfosInJsonAsync(null);
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
            var json = await GetCurranciesService.GetCurrencyInfosInJsonAsync(DateOnly.FromDateTime(date));
            return Ok(json);
        }
    }
}
