using Crawler.Core.BusinessLogics;
using Crawler.Core.BusinessLogics.Helpers;
using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Core.BusinessLogics.Services;
using Crawler.Database;
using Crawler.Database.Repositories;
using Crawler.Main.ConfigModels;
using ExchangeData.DTOModels.CrawlerToConvert;
using ExchangeData.DTOModels.CrawlerToStorage;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using MassTransit.Definition;
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
        public static void ConfigureServices(this IServiceCollection services, ConfigurationManager conf)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<AutoMapperProfile>();
            });

            services.AddScoped<GetCurranciesService>();
            services.AddScoped<BindingModelConverter>();

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
                //busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("dev", true));

                busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

               

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
                    busFactoryConfigurator.Publish<RubleQuotesByDateDTO>(x => {
                        x.Durable = true;
                        x.AutoDelete = true;
                        x.ExchangeType = "fanout";
                    });

                    busFactoryConfigurator.Publish<CurrencyInfoListDTO>(x => {
                        x.Durable = true;
                        x.AutoDelete = true;
                        x.ExchangeType = "fanout";
                    });
                    */
                    
                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}
