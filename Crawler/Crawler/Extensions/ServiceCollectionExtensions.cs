using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database;
using Crawler.Database.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
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

            /*
            services.AddDbContext<CurrencyUploadsDatesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CurrencyNotificationConnectionDbStringPostgres")));
            */

            services.AddDbContext<CurrencyUploadsDatesDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CurrencyNotificationConnectionDbStringPostgres")));

            /*
            services.AddHangfire(x => x.UseSqlServerStorage(
                configuration.GetConnectionString("CurrencyNotificationConnectionDbStringPostgres")));
            */

            services.AddHangfire(config =>
                config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(c =>
                {
                    c.UseNpgsqlConnection(configuration.GetConnectionString("CurrencyNotificationConnectionDbStringPostgres"));
                }));

            services.AddHangfireServer();

            services.AddScoped<IUploadDateRepository, UploadDateRepository>();

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
        }
    }
}
