using Converter.Main.ConfigModels;
using Converter.Main.Consumers;
using MassTransit;

namespace Converter.Main.Extensions
{
    /// <summary>
    /// Методы расширения для настройки контейнера DI.
    /// </summary>
    internal static class ServiceCollectionExtension
    {
        /// <summary>
        /// Настройка служб в контейнере DI.
        /// </summary>
        /// <param name="services">Коллекция сервисов в контейнере DI.</param>
        /// <param name="configuration">Конфигурация среды.</param>
        internal static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<RubleQuotesByDateConsumer>();

                var rabbitConfig = RabbitMQConfigModel.GetRabbitMQConfigModel(Environment.GetEnvironmentVariable("Swedlg_CurrencyExchanger_RabbitServer"));

                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host($"rabbitmq://{rabbitConfig.Url}/{rabbitConfig.Host}", cfg =>
                    {
                        cfg.Username(rabbitConfig.User);
                        cfg.Password(rabbitConfig.Password);
                    });

                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
