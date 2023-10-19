using MassTransit;
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
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
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
                //busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());
                busConfigurator.AddConsumer<CurrencyInfoCostumer>();
                busConfigurator.AddConsumer<CurrencyValueByDateConsumer>();

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
