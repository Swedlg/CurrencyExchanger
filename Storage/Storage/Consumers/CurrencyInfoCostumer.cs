using ExchangeData.DTOModels.CrawlerToStorage;
using MassTransit;
using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;

namespace Storage.Main.Consumers
{
    /// <summary>
    /// Класс слушатель DTO сообщений справочной информации о валютах.
    /// </summary>
    public class CurrencyInfoCostumer : IConsumer<CurrencyInfoListDTO>
    {
        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<CurrencyInfoCostumer> _logger;

        /// <summary>
        /// Хранилище справочной информации о валютах.
        /// </summary>
        private readonly ICurrencyInfoRepository _currencyInfoRepository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="currencyInfoRepository">Хранилище справочной информации о валютах.</param>
        public CurrencyInfoCostumer(
            ILogger<CurrencyInfoCostumer> logger,
            ICurrencyInfoRepository currencyInfoRepository)
        {
            _logger = logger;
            _currencyInfoRepository = currencyInfoRepository;
        }

        /// <summary>
        /// Обработке слушателя при получении сообщения.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <returns>:)</returns>
        public async Task Consume(ConsumeContext<CurrencyInfoListDTO> context)
        {
            _logger.LogInformation($"Получен новый объект DTO: {typeof(CurrencyInfoListDTO).Name}.");
            CurrencyInfoListDTO value = context.Message;

            foreach (var item in value.List)
            {
                bool isExist = await _currencyInfoRepository.CurrencyInfoExistsAsync(item.RId);

                if (!isExist)
                {
                    await _currencyInfoRepository.CreateCurrencyInfoAsync(new CurrencyInfoBindingModel
                    {
                        Name = item.Name,
                        EngName = item.EngName,
                        RId = item.RId,
                        ISOCharCode = item.ISOCharCode
                    });
                    _logger.LogInformation($"В БД добавлена новая запись.");
                }
            }
        }
    }
}
