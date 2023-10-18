using Hangfire.Dashboard;

namespace Crawler.Main.Extensions
{
    /// <summary>
    /// https://alimozdemir.com/posts/hangfire-docker-with-multiple-servers/
    /// </summary>
    public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Чтобы можно было авторизовываться через docker
        /// </summary>
        /// <param name="context">Контекст Dashboard для Hangfie.</param>
        /// <returns>Получилось ли авторизоваться.</returns>
        public bool Authorize(DashboardContext context)
        {
            // Allow outside. You need an authentication scenario for this part.
            // DON'T GO PRODUCTION WITH THIS LINES.
            return true;
        }
    }
}