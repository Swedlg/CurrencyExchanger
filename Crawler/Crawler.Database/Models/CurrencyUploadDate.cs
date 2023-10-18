using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crawler.Database.Models
{
    /// <summary>
    /// Последняя дата загрузки.
    /// </summary>
    [Index(nameof(Date))]
    [Table("uploaddates")]
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
