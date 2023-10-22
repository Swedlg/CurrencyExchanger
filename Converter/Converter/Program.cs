using Converter.Main.Extensions;

var builder = Host.CreateApplicationBuilder(args);

ServiceCollectionExtension.ConfigureServices(builder.Services);

IHost host = builder.Build();
await host.RunAsync();
