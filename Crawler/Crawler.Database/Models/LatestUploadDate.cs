using Crawler.Core.BusinessLogics.BindingModels;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database.Models
{
    /// <summary>
    /// Последняя дата загрузки.
    /// </summary>
    [Index(nameof(UploadDate))]
    public class LatestUploadDate : LatestUploadDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Дата выгрузки информации о котировках валют.
        /// </summary>
        public override DateOnly UploadDate { get; set; } // DateOnly (.NET) -> date (PostgreSQL)
    }
}
