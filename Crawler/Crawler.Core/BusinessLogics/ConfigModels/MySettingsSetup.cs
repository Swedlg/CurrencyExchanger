using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crawler.Core.BusinessLogics.ConfigModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MySettingsSetup : IConfigureOptions<MySettings>
    {
        /// <summary>
        /// 
        /// </summary>
        private const string SectionName = nameof(MySettings);

        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MySettingsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MySettings options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
