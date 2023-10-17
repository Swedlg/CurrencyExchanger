using Crawler.Core.BusinessLogics.BindingModels;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database.Models
{
    /// <summary>
    /// Последняя дата загрузки.
    /// </summary>
    [Index(nameof(Date))]
    public class UploadDate
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата выгрузки информации о котировках валют.
        /// </summary>
        public DateOnly Date { get; set; } // DateOnly (.NET) -> date (PostgreSQL)
    }
}
