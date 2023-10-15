namespace Converter.Main
{
    internal class ConverterService : BackgroundService
    {
        private readonly ILogger<ConverterService> _logger;

        private HttpClient? _httpClient;

        public ConverterService(ILogger<ConverterService> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker (Converter Service) running at: {time}", DateTimeOffset.Now);
                var result = await _httpClient.GetAsync("https://google.com");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"The website is up. Status code: {result.StatusCode}");
                }
                else
                {
                    _logger.LogError($"The website is down. Status code: {result.StatusCode}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
