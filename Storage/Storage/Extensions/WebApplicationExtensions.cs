using Microsoft.EntityFrameworkCore;
using Storage.Database;

namespace Storage.Main.Extensions
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
        public static void Configure(this WebApplication app, IConfiguration configuration)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CurrencyStorageDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
