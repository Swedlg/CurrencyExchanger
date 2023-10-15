using Crawler.Core.BusinessLogics.BindingModels;
using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Main.Repositories
{
    /// <summary>
    /// Репозиторий дат последних загрузок справочной информации о валютных и информации о котировках по датам
    /// </summary>
    internal class LatestUploadDateRepository : ILatestUploadDateRepository
    {
        /// <summary>
        /// Контекст базы данных с информацией о последних загрузках валют.
        /// </summary>
        private readonly LatestCurrencyUploadsDbContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных с информацией о последних загрузках валют.</param>
        public LatestUploadDateRepository(LatestCurrencyUploadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавить дату загрузки в хранилище.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли добавить дату загрузки в хранилище.</returns>
        public async Task<bool> CreateUploadDateAsync(LatestUploadDateBindingModel uploadDate)
        {
            _context.Add(uploadDate);
            return await SaveAsync();
        }

        /// <summary>
        /// Удалить дату загрузки из хранилища.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли удалить дату загрузки из хранилища.</returns>
        public async Task<bool> DeleteUploadDateAsync(LatestUploadDateBindingModel uploadDate)
        {
            _context.Remove(uploadDate);
            return await SaveAsync();
        }

        /// <summary>
        /// Асинхронный метод получения даты последней загрузки.
        /// </summary>
        /// <returns>Дата последней загрузки.</returns>
        public async Task<LatestUploadDateBindingModel?> GetUploadDateAsync()
        {
            return await _context.LatestUploadDates
                .OrderByDescending(d => d.UploadDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Асинхронный метод получения дат загрузок.
        /// </summary>
        /// <returns>Коллекция дат последних загрузок.</returns>
        public async Task<ICollection<LatestUploadDateBindingModel>> GetUploadsDatesAsync()
        {
            return (ICollection<LatestUploadDateBindingModel>) await _context.LatestUploadDates.ToListAsync();
        }

        /// <summary>
        /// Применить все изменения к хранилищу.
        /// </summary>
        /// <returns>Применились ли изменения к хранилищу.</returns>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Очистить хранилище от данных.
        /// </summary>
        /// <returns>Удалось ли очистить хранилище от дынных.</returns>
        public async Task<bool> Truncate()
        {
            _context.Database.ExecuteSql($"TRUNCATE TABLE LatestUploadDates");
            return true;
        }

        /// <summary>
        /// Асинхронный метод проверки существования даты загрузки.
        /// </summary>
        /// <param name="date">Дата зугрузки.</param>
        /// <returns>Была ли осуществлена загрузка в указанную дату.</returns>
        public async Task<bool> UploadDateExistsAsync(DateOnly date)
        {
            return await _context.LatestUploadDates.AnyAsync(d => d.UploadDate == date);
        }
    }
}
