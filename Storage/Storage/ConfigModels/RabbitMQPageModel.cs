using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Storage.Main.ConfigModels
{
    /// <summary>
    /// Модель конфигурации для RabbitMQ
    /// </summary>
    public class RabbitMQPageModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public RabbitMQPageModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ContentResult OnGet()
        {
            var rabbitMQOptions = new RabbitMQConfigModel();
            Configuration.GetSection(RabbitMQConfigModel.RabbitMQ).Bind(rabbitMQOptions);

            return Content($"Url: {rabbitMQOptions.RabbitUrl} \n" + $"Host: {rabbitMQOptions.RabbitHost} \n" +
                           $"USer: {rabbitMQOptions.RabbitUser} \n" + $"Password: {rabbitMQOptions.RabbitPassword}");
        }
    }
}
