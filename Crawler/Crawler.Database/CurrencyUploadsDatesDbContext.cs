using Crawler.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database
{
    /// <summary>
    /// Класс контекста базы данных с информацией о последних загрузках валют.
    /// </summary>
    public class CurrencyUploadsDatesDbContext : DbContext
    {
        public CurrencyUploadsDatesDbContext(DbContextOptions options) : base(options)
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
            //optionsBuilder.UseNpgsql(x => x.UseDateOnlyTimeOnly()); // Только для MSSQL
        }

        /// <summary>
        /// Даты загрузок справочной информации о валютах и валютных котировок.
        /// </summary>
        public DbSet<UploadDate> UploadDates { get; set; }
    }
}