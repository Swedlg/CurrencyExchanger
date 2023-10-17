using Converter.Main.Extensions;

var builder = Host.CreateApplicationBuilder(args);

ServiceCollectionExtension.ConfigureServices(builder.Services, builder.Configuration);

IHost host = builder.Build();
await host.RunAsync();
