using Microsoft.EntityFrameworkCore;
using Storage.Database.Models;

namespace Storage.Database
{
    /// <summary>
    /// Класс контекста данных о курсах валют
    /// </summary>
    public class CurrencyStorageDbContext : DbContext
    {
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
        public DbSet<CurrencyValueByDate> currencyValueByDates { get; set; }
    }
}