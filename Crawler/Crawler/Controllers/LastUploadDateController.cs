﻿using Crawler.Core.BusinessLogics.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Main.Controllers
{
    /// <summary>
    /// Контроллер для работы с информацией о последних датах загрузки.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LastUploadDateController : Controller
    {
        /// <summary>
        /// Хранилище дат последних загрузок.
        /// </summary>
        private readonly ILatestUploadDateRepository _latestUploadDateRepository;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        public LastUploadDateController(
            ILogger<HangfireJobController> logger,
            ILatestUploadDateRepository latestUploadDateRepository)
        {
            _logger = logger;
            _latestUploadDateRepository = latestUploadDateRepository;
        }

        /// <summary>
        /// Очистка хранилища данных о последних датах загрузок.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpDelete]
        [Route("truncate")]
        public async Task<IActionResult> Truncate()
        {
            _logger.LogInformation("Вызывается метод очистки хранилища дат последних загрузок.");
            bool truncated = await _latestUploadDateRepository.Truncate();
            if (truncated)
            {
                _logger.LogInformation("Хранилище дат последних загрузок очищено.");
            }
            return Ok();
        }
    }
}
