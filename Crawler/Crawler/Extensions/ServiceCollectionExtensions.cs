using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database;
using Crawler.Database.Repositories;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Crawler.Main.Extensions
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
           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHangfire(x => x.UseSqlServerStorage(
                configuration.GetConnectionString("CurrencyNotificationConnectionDbStringMSSQL")));

            services.AddDbContext<CurrencyUploadsDatesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CurrencyNotificationConnectionDbStringMSSQL")));

            services.AddScoped<IUploadDateRepository, UploadDateRepository>();

            services.AddHangfireServer();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

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




            /*
            var rabbitSection = configuration.GetSection("RabbitServer");
            var serviceSection = configuration.GetSection("ServiceInfo");

            ConfigureServicesMassTransit.ConfigureServices(services, configuration, new MassTransitConfiguration
            {
                IsDebug = rabbitSection.GetValue<bool>("IsDebug"),
                ServiceName = serviceSection.GetValue<string>("ServiceName"), 
            });
            */
        }
    }
}
