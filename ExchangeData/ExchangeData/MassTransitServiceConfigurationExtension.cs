using Calabonga.Microservices.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeData
{
    public static class MassTransitServiceConfigurationExtension
    {
        public static void Configure(
            this IServiceCollection services,
            Action<MassTransitConfiguration> configuration,
            string serviceName)
        {
            var transitConfiguration = new MassTransitConfiguration();

            if (configuration == null)
            {
                throw new MicroserviceArgumentNullException(nameof(configuration));
            }

            configuration(transitConfiguration);

            if (string.IsNullOrEmpty(serviceName))
            {
                throw new MicroserviceArgumentNullException(transitConfiguration.ServiceName);
            }

            transitConfiguration.ServiceName = serviceName;
            services.AddSingleton(transitConfiguration);
        }
    }
}
