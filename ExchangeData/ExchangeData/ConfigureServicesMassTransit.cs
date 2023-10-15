using Calabonga.Microservices.Core.Exceptions;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeData
{
    public static class ConfigureServicesMassTransit
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration,
            MassTransitConfiguration massTransitConfiguration)
        {
            if (massTransitConfiguration == null || massTransitConfiguration.IsDebug)
            {
                return;
            }

            var rabbitSection = configuration.GetSection("RabbitServer");
            var url = rabbitSection.GetValue<string>("Url");
            var host = rabbitSection.GetValue<string>("Host");

            if (rabbitSection == null || string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(url))
            {
                throw new MicroserviceArgumentNullException("AppSetting does not contains data for Rabbit");
            }

            services.AddMassTransit(x =>
            {
                x.AddBus(busFactory =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host($"rabbitmq://{url}/{host}", configurator =>
                        {
                            configurator.Username("currency-exchanger-guest");
                            configurator.Password("currency-exchanger-guest");
                        });

                        cfg.ConfigureEndpoints(busFactory, KebabCaseEndpointNameFormatter.Instance);
                    });

                    massTransitConfiguration.BusControl?.Invoke(bus, services.BuildServiceProvider());
                    return bus;
                });

                massTransitConfiguration.Configurator?.Invoke(x); 
                services.AddMassTransitHostedService();
            });
        }
    }
}
