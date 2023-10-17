using Microsoft.EntityFrameworkCore;
using Storage.Database.Models;

namespace Storage.Database
{
    /// <summary>
    /// Класс контекста данных о курсах валют
    /// </summary>
    public class CurrencyStorageDbContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Опции.</param>
        public CurrencyStorageDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Справочник валют.
        /// </summary>
        public DbSet<CurrencyInfo> СurrencyInfos { get; set; }

        /// <summary>
        /// Значения валют по датам.
        /// </summary>
        public DbSet<CurrencyValueByDate> CurrencyValueByDates { get; set; }
    }
}