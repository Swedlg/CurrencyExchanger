namespace Crawler.Main.ConfigModels
{
    /// <summary>
    /// Пользовательские настройки.
    /// </summary>
    public class MySettings
    {
        /// <summary>
        /// Максимальное количество дней от текущего дня для начала загрузки.
        /// </summary>
        public int SinceDaysCount { get; set; }
    }
}