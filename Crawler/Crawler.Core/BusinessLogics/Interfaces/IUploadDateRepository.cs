using Crawler.Core.BusinessLogics.BindingModels;

namespace Crawler.Core.BusinessLogics.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория дат последних загрузок.
    /// </summary>
    public interface IUploadDateRepository
    {
        /// <summary>
        /// Получить список всех дат загрузок информации о валютах.
        /// </summary>
        /// <returns>Коллекция дат последних загрузок.</returns>
        Task<ICollection<UploadDateBindingModel>> GetUploadsDatesAsync();

        /// <summary>
        /// Получить последнюю дату загрузки информации о валютах.
        /// </summary>
        /// <returns>Дата последней загрузки информации о валютах.</returns>
        Task<UploadDateBindingModel?> GetUploadDateAsync();

        /// <summary>
        /// Существует ли дата загруки информации о валютах в хранилище.
        /// </summary>
        /// <param name="date">Дата зугрузки.</param>
        /// <returns>Существует ли дата загруки информации о валютах в хранилище.</returns>
        Task<bool> UploadDateExistsAsync(DateOnly date);

        /// <summary>
        /// Добавить дату загрузки информации о валюте в хранилище.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки информации о валюте.</param>
        /// <returns>Удалось ли добавить дату загрузки информации о валюте в хранилище.</returns>
        Task<bool> CreateUploadDateAsync(UploadDateBindingModel uploadDate);

        /// <summary>
        /// Удалить дату загрузки информации о валюте из хранилища.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли удалить дату загрузки  информации о валюте из хранилища.</returns>
        Task<bool> DeleteUploadDateAsync(UploadDateBindingModel uploadDate);

        /// <summary>
        /// Применить все изменения к хранилищу.
        /// </summary>
        /// <returns>Применились ли изменения к хранилищу.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Очистить хранилище от данных.
        /// </summary>
        /// <returns>Удалось ли очистить хранилище от данных.</returns>
        Task<bool> Truncate();
    }
}
