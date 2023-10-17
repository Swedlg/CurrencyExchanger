using Storage.Core.BusinessLogics.BindingModels;

namespace Storage.Core.BusinessLogics.Interfaces
{
    /// <summary>
    /// Интерфейс хранилища информации о валютах котировках по датам.
    /// </summary>
    public interface ICurrencyValueByDateRepository
    {
        /// <summary>
        /// Получить список информации о валютных котировках с фильтрацией по диапазону дат и кодам валют.
        /// </summary>
        /// <param name="dateFrom">Начальная дата для фильтрации.</param>
        /// <param name="dateTo">Конечная дата для фильтрации</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Список информации о валютных котировках с фильтрацией по диапазону дат и кодам валют.</returns>
        Task<ICollection<CurrencyValueByDateBindingModel>> GetFilteredAsync(
            DateOnly? dateFrom,
            DateOnly? dateTo,
            string BaseRId,
            string? OtherRId);

        /// <summary>
        /// Получит информацию о валютной котировке.
        /// </summary>
        /// <param name="date">Дата валютной котировки.</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Информация о валютной котировке</returns>
        Task<CurrencyValueByDateBindingModel?> GetCurrencyValueByDateAsync(DateOnly date, string BaseRId, string OtherRId);

        /// <summary>
        /// Существует ли информация о валютной котировке.
        /// </summary>
        /// <param name="date">Дата валютной котировки.</param>
        /// <param name="BaseRId">RId базовой валюты.</param>
        /// <param name="OtherRId">RId другой валюты.</param>
        /// <returns>Существует ли информаци яо валютной котировке.</returns>
        Task<bool> CurrencyValueByDateExistsAsync(DateOnly date, string BaseRId, string OtherRId);

        /// <summary>
        /// Создать запись информации о валютной котировке.
        /// </summary>
        /// <param name="item">Информация о валютной котировке.</param>
        /// <returns>Удалось ли создать запись информации о валютной котировке.</returns>
        Task<bool> CreateCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item);

        /// <summary>
        /// Удалить запись информации о валютной котировке.
        /// </summary>
        /// <param name="item">Информация о валютной котировке.</param>
        /// <returns>Удалось ли удалить запись информации о валютной котировке.</returns>
        Task<bool> DeleteCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item);

        /// <summary>
        /// Применить изменения в хранилище.
        /// </summary>
        /// <returns>Удалось ли применить изменения в хранилище.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Очистить таблицу с информацией о валютных котировках.
        /// </summary>
        /// <returns>Удалось ли очистить таблицу с информацией о валютных котировках.</returns>
        Task<bool> Truncate();
    }
}
