using Crawler.Core.BusinessLogics.BindingModels;
using Crawler.Core.BusinessLogics.Interfaces;
using Crawler.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database.Repositories
{
    /// <summary>
    /// Репозиторий дат последних загрузок справочной информации о валютных и информации о котировках по датам
    /// </summary>
    public class UploadDateRepository : IUploadDateRepository
    {
        /// <summary>
        /// Контекст базы данных с информацией о последних загрузках валют.
        /// </summary>
        private readonly CurrencyUploadsDatesDbContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных с информацией о последних загрузках валют.</param>
        public UploadDateRepository(CurrencyUploadsDatesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавить дату загрузки в хранилище.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли добавить дату загрузки в хранилище.</returns>
        public async Task<bool> CreateUploadDateAsync(UploadDateBindingModel uploadDate)
        {
            _context.Add(ParseToDbModel(uploadDate));
            return await SaveAsync();
        }

        /// <summary>
        /// Удалить дату загрузки из хранилища.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли удалить дату загрузки из хранилища.</returns>
        public async Task<bool> DeleteUploadDateAsync(UploadDateBindingModel uploadDate)
        {
            _context.Remove(ParseToDbModel(uploadDate));
            return await SaveAsync();
        }

        /// <summary>
        /// Асинхронный метод получения даты последней загрузки.
        /// </summary>
        /// <returns>Дата последней загрузки.</returns>
        public async Task<UploadDateBindingModel?> GetUploadDateAsync()
        {
            return await _context.UploadDates
                .OrderByDescending(d => d.Date)
                .Select(u => new UploadDateBindingModel
                {
                    Id = u.Id,
                    UploadDate = u.Date
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Асинхронный метод получения дат загрузок.
        /// </summary>
        /// <returns>Коллекция дат последних загрузок.</returns>
        public async Task<ICollection<UploadDateBindingModel>> GetUploadsDatesAsync()
        {
            return await _context.UploadDates
                .Select(u => new UploadDateBindingModel
                {
                    Id = u.Id,
                    UploadDate = u.Date
                }).ToListAsync();
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
            _context.Database.ExecuteSql($"TRUNCATE TABLE UploadDates");
            return true;
        }

        /// <summary>
        /// Асинхронный метод проверки существования даты загрузки.
        /// </summary>
        /// <param name="date">Дата зугрузки.</param>
        /// <returns>Была ли осуществлена загрузка в указанную дату.</returns>
        public async Task<bool> UploadDateExistsAsync(DateOnly date)
        {
            return await _context.UploadDates.AnyAsync(d => d.Date == date);
        }

        /// <summary>
        /// Запарсить Binding модель даты загрузки в Db модель даты загрузки.
        /// </summary>
        /// <param name="bindingModel">Binding модель даты загрузки.</param>
        /// <returns>Db модель даты загрузки.</returns>
        private UploadDate ParseToDbModel(UploadDateBindingModel bindingModel)
        {
            return new UploadDate
            {
                Id = bindingModel.Id,
                Date = bindingModel.UploadDate,
            };
        }
    }
}
