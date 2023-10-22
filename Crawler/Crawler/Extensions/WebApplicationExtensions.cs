using Crawler.Database;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Main.Extensions
{
    /// <summary>
    /// Методы расширения для веб-приложения.
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Конфигурация Pipeline.
        /// </summary>
        /// <param name="app">Настраиваемое веб-прилоежние.</param>
        /// <param name="configuration">Конфигурация среды.</param>
        public static void Configure(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<CurrencyUploadsDatesDbContext>();
            context.Database.Migrate();
        }
    }
}
