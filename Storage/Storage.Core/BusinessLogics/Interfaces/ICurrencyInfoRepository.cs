using Storage.Core.BusinessLogics.BindingModels;

namespace Storage.Core.BusinessLogics.Interfaces
{
    /// <summary>
    /// Интерфейс хранилища справочной информации о валютах.
    /// </summary>
    public interface ICurrencyInfoRepository
    {
        /// <summary>
        /// Получить список справочной информации о всех валютах.
        /// </summary>
        /// <returns>Список справочной информации о всех валютах.</returns>
        Task<ICollection<CurrencyInfoBindingModel>> GetAllCurrencyInfoAsync();

        /// <summary>
        /// Получить справочную информацию о валюте по ее RId.
        /// </summary>
        /// <param name="RId">RId валюты.</param>
        /// <returns>Справочная информация валюте.</returns>
        Task<CurrencyInfoBindingModel?> GetCurrencyInfoByRIdAsync(string RId);

        /// <summary>
        /// Существует ли справочная информация о валюте (по ее RId).
        /// </summary>
        /// <param name="RId">RId валюты.</param>
        /// <returns>Существует ли справочная информация о валютe.</returns>
        Task<bool> CurrencyInfoExistsAsync(string RId);

        /// <summary>
        /// Создать запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли создать запись.</returns>
        Task<bool> CreateCurrencyInfoAsync(CurrencyInfoBindingModel item);

        /// <summary>
        /// Удалить запись справочной информации о валюте.
        /// </summary>
        /// <param name="item">Модель валюты.</param>
        /// <returns>Удалось ли удалить запись.</returns>
        Task<bool> DeleteCurrencyInfoAsync(CurrencyInfoBindingModel item);

        /// <summary>
        /// Применить изменения в хранилище.
        /// </summary>
        /// <returns>Удалось ли применить изменения в хранилище.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Очистить таблицу со справочной информацией о валютах.
        /// </summary>
        /// <returns>Удалось ли очистить таблицу со справочной информацией о валютах.</returns>
        Task<bool> Truncate();
    }
}
