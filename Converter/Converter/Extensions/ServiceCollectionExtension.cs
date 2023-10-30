using Converter.Main.ConfigModels;
using Converter.Main.Consumers;
using ExchangeData.DTOModels.ConvertToStorage;
using ExchangeData.DTOModels.CrawlerToStorage;
using MassTransit;
using MassTransit.Definition;

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
        internal static void ConfigureServices(this IServiceCollection services, ConfigurationManager conf)
        {
            services.AddMassTransit(busConfigurator =>
            {
                //busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("dev", true));

                busConfigurator.AddConsumer<RubleQuotesByDateConsumer>();
                /*
                .Endpoint(e =>
                {
                    e.Temporary = true;
                    e.InstanceId = "1";
                });
                */

                var rabbitMQOptions = new RabbitMQConfigModel();
                conf.GetSection(RabbitMQConfigModel.RabbitMQ).Bind(rabbitMQOptions);

                services.Configure<RabbitMQConfigModel>(conf.GetSection(RabbitMQConfigModel.RabbitMQ));

                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host($"rabbitmq://{rabbitMQOptions.RabbitUrl}/{rabbitMQOptions.RabbitHost}", cfg =>
                    {
                        cfg.Username(rabbitMQOptions.RabbitUser);
                        cfg.Password(rabbitMQOptions.RabbitPassword);
                    });

                    /*
                    busFactoryConfigurator.Publish<CurrencyQuotesByDateListDTO>(x => {
                        x.Durable = true;
                        x.AutoDelete = true;
                        x.ExchangeType = "fanout";
                    });
                    */

                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
