using Microsoft.EntityFrameworkCore;
using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;
using Storage.Database.Models;

namespace Storage.Database.Repositories
{
    /// <summary>
    /// Хранилище справочной информации о валютах.
    /// </summary>
    public class CurrencyInfoRepository : ICurrencyInfoRepository
    {
        /// <summary>
        /// Контекст базы данных со справочной информацией о валютах.
        /// </summary>
        private readonly CurrencyStorageDbContext _context;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст базы данных со справочной информацией о валютах.</param>
        public CurrencyInfoRepository(CurrencyStorageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создать запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли создать запись.</returns>
        public async Task<bool> CreateCurrencyInfoAsync(CurrencyInfoBindingModel item)
        {
            _context.Add(ParseToDbModel(item));
            return await SaveAsync();
        }

        /// <summary>
        /// Существует ли справочная информация о валюте (по ее RId).
        /// </summary>
        /// <param name="RId">RId валюты.</param>
        /// <returns>Существует ли справочная информация о валютe.</returns>
        public async Task<bool> CurrencyInfoExistsAsync(string RId)
        {
            return await _context.СurrencyInfos.AnyAsync(c => c.RId == RId);
        }

        /// <summary>
        /// Удалить запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли удалить запись.</returns>
        public async Task<bool> DeleteCurrencyInfoAsync(CurrencyInfoBindingModel item)
        {
            _context.Remove(ParseToDbModel(item));
            return await SaveAsync();
        }

        /// <summary>
        /// Получить список справочной информации о всех валютах.
        /// </summary>
        /// <returns>Список справочной информации о всех валютах.</returns>
        public async Task<ICollection<CurrencyInfoBindingModel>> GetAllCurrencyInfoAsync()
        {
            return (ICollection<CurrencyInfoBindingModel>)await _context.СurrencyInfos
                .Select(c => new CurrencyInfoBindingModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    EngName = c.EngName,
                    RId = c.RId,
                    ISOCharCode = c.ISOCharCode,
                }).ToListAsync();
        }

        /// <summary>
        /// Получить справочную информацию о валюте по ее RId.
        /// </summary>
        /// <param name="RId">RId валюты.</param>
        /// <returns>Справочная информация валюте.</returns>
        public async Task<CurrencyInfoBindingModel?> GetCurrencyInfoByRIdAsync(string RId)
        {
            return await _context.СurrencyInfos
                .Where(c => String.Equals(c.RId, RId))
                .Select(c => new CurrencyInfoBindingModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    EngName = c.EngName,
                    RId = c.RId,
                    ISOCharCode = c.ISOCharCode,
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Применить изменения в хранилище.
        /// </summary>
        /// <returns>Удалось ли применить изменения в хранилище.</returns>
        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Очистить таблицу со справочной информацией о валютах.
        /// </summary>
        /// <returns>Удалось ли очистить таблицу со справочной информацией о валютах.</returns>
        public async Task<bool> Truncate()
        {
            await _context.Database.ExecuteSqlAsync($"TRUNCATE currencyinfos CASCADE");
            return true;
        }

        /// <summary>
        /// Запарсить Binding модель cправочной информации о валюте в Db модель cправочной информации о валюте.
        /// </summary>
        /// <param name="bindingModel">Binding модель справочной информации о валюте.</param>
        /// <returns>Db модель справочной информации о валюте.</returns>
        private CurrencyInfo ParseToDbModel(CurrencyInfoBindingModel bindingModel)
        {
            return new CurrencyInfo
            {
                Id = bindingModel.Id,
                Name = bindingModel.Name,
                EngName = bindingModel.EngName,
                RId = bindingModel.RId,
                ISOCharCode = bindingModel.ISOCharCode,
            };
        }
    }
}
