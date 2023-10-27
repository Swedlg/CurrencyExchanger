using MassTransit;
using MassTransit.Definition;
using Microsoft.EntityFrameworkCore;
using Storage.Core.BusinessLogics.Interfaces;
using Storage.Database;
using Storage.Database.Repositories;
using Storage.Main.ConfigModels;
using Storage.Main.Consumers;

namespace Storage.Main.Extensions
{
    /// <summary>
    /// Методы расширения для настройки контейнера DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Настройка служб в контейнере DI.
        /// </summary>
        /// <param name="services">Коллекция сервисов в контейнере DI.</param>
        /// <param name="configuration">Конфигурация среды.</param>
        public static void ConfigureServices(this IServiceCollection services, ConfigurationManager conf)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddLogging();

            services.AddDbContext<CurrencyStorageDbContext>(options =>
                        options.UseNpgsql(Environment.GetEnvironmentVariable("Swedlg_CurrencyExchanger_CurrencyStorageConnectionDbString")));

            services.AddScoped<ICurrencyInfoRepository, CurrencyInfoRepository>();
            services.AddScoped<ICurrencyValueByDateRepository, CurrencyValueByDateRepository>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<CurrencyInfoCostumer>();

                busConfigurator.AddConsumer<CurrencyValueByDateConsumer>();

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

                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
