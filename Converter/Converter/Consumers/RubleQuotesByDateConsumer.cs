using Converter.Core.BusinessLogics.Services;
using ExchangeData.DTOModels.ConvertToStorage;
using ExchangeData.DTOModels.CrawlerToConvert;
using MassTransit;

namespace Converter.Main.Consumers
{
    /// <summary>
    /// Слушатель объектов RubleQuotesByDateDTO
    /// </summary>
    public class RubleQuotesByDateConsumer : IConsumer<RubleQuotesByDateDTO>
    {
        /// <summary>
        /// Логгер.
        /// </summary>
        public readonly ILogger<RubleQuotesByDateConsumer> _logger;

        /// <summary>
        /// Слушатель Endpoint'ов для MassTransit + RabbitMQ.
        /// </summary>
        public readonly IPublishEndpoint _publishEndpoint;

        public RubleQuotesByDateConsumer(
            ILogger<RubleQuotesByDateConsumer> logger,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Обработчик получения модели RubleQuotesByDateDTO.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <returns>:)</returns>
        public async Task Consume(ConsumeContext<RubleQuotesByDateDTO> context)
        {
            _logger.LogInformation("Получен новый объект DTO: {typeof(RubleQuotesByDateDTO).Name}.", typeof(RubleQuotesByDateDTO).Name);
            CurrencyQuotesByDateListDTO currencyQuotesByDateListDTO = DTOConverter.ConvertToCurrencyQuotesByDateListDTO(context.Message.List, context.Message.Date);
            await _publishEndpoint.Publish(currencyQuotesByDateListDTO);
            _logger.LogInformation("Объект {typeof(RubleQuotesByDateDTO).Name} обработан и передан в Storage.", typeof(RubleQuotesByDateDTO).Name);
        }
    }
}
