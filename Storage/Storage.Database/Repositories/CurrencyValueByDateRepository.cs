using Microsoft.EntityFrameworkCore;
using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;
using Storage.Database.Models;

namespace Storage.Database.Repositories
{
    /// <summary>
    /// Хранилище информации о валютах котировках по датам.
    /// </summary>
    public class CurrencyValueByDateRepository : ICurrencyValueByDateRepository
    {
        /// <summary>
        /// Контекст базы данных со справочной информацией о валютах.
        /// </summary>
        private readonly CurrencyStorageDbContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных со справочной информацией о валютах.</param>
        public CurrencyValueByDateRepository(CurrencyStorageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создать запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли создать запись.</returns>
        public async Task<bool> CreateCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item)
        {
            _context.Add(ParseToDbModel(item));
            return await SaveAsync();
        }

        /// <summary>
        /// Существует ли информация о валютной котировке.
        /// </summary>
        /// <param name="date">Дата валютной котировки.</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Существует ли информаци яо валютной котировке.</returns>
        public async Task<bool> CurrencyValueByDateExistsAsync(DateOnly date, string BaseRId, string OtherRId)
        {
            return await _context.CurrencyValueByDates
                .AnyAsync(c => c.Date == date && c.BaseCurrency.RId == BaseRId && c.Currency.RId == OtherRId);
        }

        /// <summary>
        /// Удалить запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли удалить запись.</returns>
        public async Task<bool> DeleteCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item)
        {
            _context.Remove(ParseToDbModel(item));
            return await SaveAsync();
        }

        /// <summary>
        /// Получит информацию о валютной котировке.
        /// </summary>
        /// <param name="date">Дата валютной котировки.</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Информация о валютной котировке</returns>
        public async Task<CurrencyValueByDateBindingModel?> GetCurrencyValueByDateAsync(DateOnly date, string BaseRId, string OtherRId)
        {
            return await _context.CurrencyValueByDates
                .Where(c => c.Date == date && c.BaseCurrency.RId == BaseRId && c.Currency.RId == OtherRId)
                .Select(c => new CurrencyValueByDateBindingModel
                {
                    Id = c.Id,
                    BaseCurrencyId = c.BaseCurrencyId,
                    CurrencyId = c.CurrencyId,
                    Date = c.Date,
                    Value = c.Value
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получить список информации о валютных котировках с фильтрацией по диапазону дат и кодам валют.
        /// </summary>
        /// <param name="dateFrom">Начальная дата для фильтрации.</param>
        /// <param name="dateTo">Конечная дата для фильтрации</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Список информации о валютных котировках с фильтрацией по диапазону дат и кодам валют.</returns>
        public async Task<ICollection<CurrencyValueByDateBindingModel>> GetFilteredAsync(DateOnly? dateFrom, DateOnly? dateTo, string BaseRId, string OtherRId)
        {
            if (dateFrom == null || dateTo == null)
            {
                if (!string.IsNullOrWhiteSpace(OtherRId))
                {
                    return await _context.CurrencyValueByDates
                        .Where(c => c.BaseCurrency.RId == BaseRId && c.Currency.RId == OtherRId)
                        .Select(c => new CurrencyValueByDateBindingModel
                        {
                            Id = c.Id,
                            BaseCurrencyId = c.BaseCurrencyId,
                            CurrencyId = c.CurrencyId,
                            Date = c.Date,
                            Value = c.Value
                        })
                        .ToListAsync();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(OtherRId))
                {
                    return (ICollection<CurrencyValueByDateBindingModel>)await _context.CurrencyValueByDates
                        .Where(c => c.BaseCurrency.RId == BaseRId && c.Currency.RId == OtherRId && c.Date > dateFrom && c.Date < dateTo)
                        .Select(c => new CurrencyValueByDateBindingModel
                        {
                            Id = c.Id,
                            BaseCurrencyId = c.BaseCurrencyId,
                            CurrencyId = c.CurrencyId,
                            Date = c.Date,
                            Value = c.Value
                        })
                        .ToListAsync();
                }
                else
                {
                    return (ICollection<CurrencyValueByDateBindingModel>)await _context.CurrencyValueByDates
                        .Where(c => c.BaseCurrency.RId == BaseRId && c.Date > dateFrom && c.Date < dateTo)
                        .Select(c => new CurrencyValueByDateBindingModel
                        {
                            Id = c.Id,
                            BaseCurrencyId = c.BaseCurrencyId,
                            CurrencyId = c.CurrencyId,
                            Date = c.Date,
                            Value = c.Value
                        })
                        .ToListAsync();
                }
            }

            return new List<CurrencyValueByDateBindingModel> { };
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
        /// Очистить таблицу с информацией о валютных котировках.
        /// </summary>
        /// <returns>Удалось ли очистить таблицу с информацией о валютных котировках.</returns>
        public async Task<bool> Truncate()
        {
            await _context.Database.ExecuteSqlAsync($"TRUNCATE TABLE currencyvaluebydates");
            return true;
        }

        /// <summary>
        /// Запарсить Binding модель котировки валюты в Db модель котировки валюты.
        /// </summary>
        /// <param name="bindingModel">Binding модель котировки валюты.</param>
        /// <returns>Db модель котировки валюты.</returns>
        private CurrencyValueByDate ParseToDbModel(CurrencyValueByDateBindingModel bindingModel)
        {
            return new CurrencyValueByDate
            {
                Id = bindingModel.Id,
                BaseCurrencyId = bindingModel.BaseCurrencyId,
                CurrencyId = bindingModel.CurrencyId,
                Date = bindingModel.Date,
                Value = bindingModel.Value,
            };
        }
    }
}
