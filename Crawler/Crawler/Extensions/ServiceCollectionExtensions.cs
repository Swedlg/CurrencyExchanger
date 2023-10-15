using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database;
using Crawler.Main.Repositories;
using ExchangeData;
using Hangfire;
using Microsoft.EntityFrameworkCore;

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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHangfire(x => x.UseSqlServerStorage(
                configuration.GetConnectionString("CurrencyNotificationConnectionDbStringMSSQL")));

            services.AddDbContext<LatestCurrencyUploadsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CurrencyNotificationConnectionDbStringMSSQL")));

            services.AddScoped<ILatestUploadDateRepository, LatestUploadDateRepository>();

            services.AddHangfireServer();

            var rabbitSection = configuration.GetSection("RabbitServer");
            var serviceSection = configuration.GetSection("ServiceInfo");
            ConfigureServicesMassTransit.ConfigureServices(services, configuration, new MassTransitConfiguration
            {
                IsDebug = rabbitSection.GetValue<bool>("IsDebug"),
                ServiceName = serviceSection.GetValue<string>("ServiceName"), 
            });
        }
    }
}
