using ExchangeData.DTOModels.ConvertToStorage;
using ExchangeData.DTOModels.CrawlerToStorage;
using MassTransit;
using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;

namespace Storage.Main.Consumers
{
    /// <summary>
    /// Слушатель CurrencyQuotationByDateDTO.
    /// </summary>
    public class CurrencyValueByDateConsumer : IConsumer<CurrencyQuotesByDateListDTO>
    {
        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<CurrencyValueByDateConsumer> _logger;

        /// <summary>
        /// Хранилище справочной информации о валютах.
        /// </summary>
        private readonly ICurrencyInfoRepository _currencyInfoRepository;

        /// <summary>
        /// Хранилище информации о валютных котировках по датам.
        /// </summary>
        private readonly ICurrencyValueByDateRepository _currencyValueByDateRepository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="currencyInfoRepository">Хранилище справочной информации о валютах.</param>
        /// <param name="currencyValueByDateRepository">Хранилище информации о валютных котировках по датам.</param>
        public CurrencyValueByDateConsumer(
            ILogger<CurrencyValueByDateConsumer> logger,
            ICurrencyInfoRepository currencyInfoRepository,
            ICurrencyValueByDateRepository currencyValueByDateRepository)
        {
            _logger = logger;
            _currencyInfoRepository = currencyInfoRepository;
            _currencyValueByDateRepository = currencyValueByDateRepository;
        }

        /// <summary>
        /// Обработке слушателя при получении сообщения.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <returns>:)</returns>
        public async Task Consume(ConsumeContext<CurrencyQuotesByDateListDTO> context)
        {
            _logger.LogInformation("Получен новый объект DTO: {typeof(CurrencyInfoListDTO).Name}.", typeof(CurrencyInfoListDTO).Name);
            CurrencyQuotesByDateListDTO currencyQuotesByDateListDTO = context.Message;

            foreach(var item in currencyQuotesByDateListDTO.List)
            {
                bool isExist = await _currencyValueByDateRepository.CurrencyValueByDateExistsAsync(DateOnly.FromDateTime(item.Date), item.BaseCurrencyId, item.CurrencyId);

                if (!isExist)
                {
                    CurrencyInfoBindingModel? baseId = await _currencyInfoRepository.GetCurrencyInfoByRIdAsync(item.BaseCurrencyId);
                    CurrencyInfoBindingModel? otherId = await _currencyInfoRepository.GetCurrencyInfoByRIdAsync(item.CurrencyId);

                    if (baseId != null && otherId != null)
                    {
                        await _currencyValueByDateRepository.CreateCurrencyValueByDateAsync(new CurrencyValueByDateBindingModel
                        {
                            Date = DateOnly.FromDateTime(item.Date),
                            BaseCurrencyId = baseId.Id,
                            CurrencyId = otherId.Id,
                            Value = item.Value,
                        });
                        _logger.LogInformation($"В БД добавлена новая запись.");
                    }
                }
            } 
        }
    }
}
