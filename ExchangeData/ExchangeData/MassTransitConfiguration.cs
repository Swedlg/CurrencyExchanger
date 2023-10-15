using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace ExchangeData
{
    /// <summary>
    /// 
    /// </summary>
    public class MassTransitConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        /// Конфигуратор, позволяющий конкретному сервису вносить изменения о подключении к RabbitMQ
        /// </summary>
        public Action<IServiceCollectionBusConfigurator> Configurator { get; set; }

        public Action<IBusControl, IServiceProvider> BusControl { get; set; }

        /// <summary>
        /// Имя подключаемого сервиса
        /// </summary>
        public string ServiceName { get; set; }
    }
}
