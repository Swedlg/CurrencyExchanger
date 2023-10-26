 using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database;
using Crawler.Database.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Crawler.Core.BusinessLogics;
using Crawler.Core.BusinessLogics.Services;
using Crawler.Core.BusinessLogics.Helpers;
using Crawler.Main.CongifModels;

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
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<AutoMapperProfile>();
            });

            services.AddScoped<GetCurranciesService>();
            services.AddScoped<XmlParseHelper>();
            services.AddScoped<JsonParseHelper>();

            services.AddHttpClient();

            services.AddDbContext<CurrencyUploadsDatesDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("Swedlg_CurrencyExchanger_CurrencyNotificationConnectionDbStringPostgres")));

            services.AddHangfire(config =>
                config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(c =>
                {
                    c.UseNpgsqlConnection(Environment.GetEnvironmentVariable("Swedlg_CurrencyExchanger_CurrencyNotificationConnectionDbStringPostgres"));
                }));

            services.AddHangfireServer();

            

            services.AddScoped<IUploadDateRepository, UploadDateRepository>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

                var rabbitConfig = RabbitMQConfigModel.GetRabbitMQConfigModel(Environment.GetEnvironmentVariable("Swedlg_CurrencyExchanger_RabbitServer"));

                Console.WriteLine($"{rabbitConfig.Url} {rabbitConfig.Host} {rabbitConfig.User} {rabbitConfig.Password}");

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
        }
    }
}
