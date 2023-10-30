﻿namespace Storage.Main.ConfigModels
{
    public class RabbitMQConfigModel
    {
        public const string RabbitMQ = "RabbitMQ";

        /// <summary>
        /// Url.
        /// </summary>
        public string RabbitUrl { get; set; } = String.Empty;

        /// <summary>
        /// Виртуальный хост.
        /// </summary>
        public string RabbitHost { get; set; } = String.Empty;

        /// <summary>
        /// Пользователь.
        /// </summary>
        public string RabbitUser { get; set; } = String.Empty;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string RabbitPassword { get; set; } = String.Empty;
    }
}
