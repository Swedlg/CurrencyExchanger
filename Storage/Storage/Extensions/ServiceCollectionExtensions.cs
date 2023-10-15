using ExchangeData;
using Microsoft.EntityFrameworkCore;
using Storage.Core.BusinessLogics.Interfaces;
using Storage.Database;
using Storage.Repositories;

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

            services.AddDbContext<CurrencyStorageDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("CurrencyStorageConnectionDbString")));

            services.AddScoped<ICurrencyInfoRepository, CurrencyInfoRepository>();
            services.AddScoped<ICurrencyValueByDateRepository, CurrencyValueByDateRepository>();

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
