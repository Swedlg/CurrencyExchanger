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

                var rabbitSection = configuration.GetSection("RabbitServer");

                string url = rabbitSection.GetValue<string>("Url");
                string host = rabbitSection.GetValue<string>("Host");
                string user = rabbitSection.GetValue<string>("User");
                string password = rabbitSection.GetValue<string>("Password");

                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host($"rabbitmq://{url}/{host}", cfg =>
                    {
                        cfg.Username("currency-exchanger-guest");
                        cfg.Password("currency-exchanger-guest");
                    });

                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
