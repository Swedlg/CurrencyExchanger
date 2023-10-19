using Newtonsoft.Json;

namespace Crawler.Main.CongifModels
{
    /// <summary>
    /// Модель конфигурации для RabbitMQ
    /// </summary>
    public class RabbitMQConfigModel
    {
        #region Значения беруться из переменной среды. Тут указаны просто значения по умолчанию

        /// <summary>
        /// Url.
        /// </summary>
        public string Url { get; set; } = "localhost";

        /// <summary>
        /// Виртуальный хост.
        /// </summary>
        public string Host { get; set; } = "/";

        /// <summary>
        /// Пользователь.
        /// </summary>
        public string User { get; set; } = "guest";

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; } = "guest";

        #endregion

        public static RabbitMQConfigModel GetRabbitMQConfigModel(string? json)
        {
            if (json != null)
            {
                try
                {
                    dynamic? rabbitConfig = JsonConvert.DeserializeObject(json);
                    if (rabbitConfig != null)
                    {
                        return rabbitConfig.RabbitServer.ToObject<RabbitMQConfigModel>() ?? new RabbitMQConfigModel();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }        
            }
            return new RabbitMQConfigModel();
        } 
    }
}
