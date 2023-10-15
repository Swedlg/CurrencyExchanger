using ExchangeData;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Converter.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\temp\workerservice\LogFile.txt")
                .CreateLogger();
            try
            {
                Log.Information("Starting up the service");
                CreateHostBulder(args).Build().Run();
                return;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBulder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ConverterService>();

                    IConfiguration configuration = hostContext.Configuration;

                    var rabbitSection = configuration.GetSection("RabbitServer");
                    var serviceSection = configuration.GetSection("ServiceInfo");
                    ConfigureServicesMassTransit.ConfigureServices(services, configuration, new MassTransitConfiguration
                    {
                        IsDebug = rabbitSection.GetValue<bool>("IsDebug"),
                        ServiceName = serviceSection.GetValue<string>("ServiceName"),
                    });
                })
                .UseSerilog();

            return builder;
        }
    }
}
