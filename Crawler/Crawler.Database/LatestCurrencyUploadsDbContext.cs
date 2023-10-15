using Crawler.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database
{
    /// <summary>
    /// Класс контекста базы данных с информацией о последних загрузках валют.
    /// </summary>
    public class LatestCurrencyUploadsDbContext : DbContext
    {
        public LatestCurrencyUploadsDbContext(DbContextOptions options) : base(options)
        {
            
        }

        /// <summary>
        /// Настройка параметров подключения к БД.
        /// </summary>
        /// <param name="optionsBuilder">Параметры подключения к БД.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Поддержка DateOnly типов (зависимость ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly подключена именно для этого)
            optionsBuilder.UseSqlServer(x => x.UseDateOnlyTimeOnly()); 
        }

        /// <summary>
        /// Последние загрузки валют
        /// </summary>
        public DbSet<LatestUploadDate> LatestUploadDates { get; set; }
    }
}