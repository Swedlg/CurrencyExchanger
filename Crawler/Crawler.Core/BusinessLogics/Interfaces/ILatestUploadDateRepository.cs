using Crawler.Core.BusinessLogics.BindingModels;

namespace Crawler.Core.BusinessLogics.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория дат последних загрузок.
    /// </summary>
    public interface ILatestUploadDateRepository
    {
        /// <summary>
        /// Асинхронный метод получения дат загрузок.
        /// </summary>
        /// <returns>Коллекция дат последних загрузок.</returns>
        Task<ICollection<LatestUploadDateBindingModel>> GetUploadsDatesAsync();

        /// <summary>
        /// Асинхронный метод получения даты последней загрузки.
        /// </summary>
        /// <returns>Дата последней загрузки.</returns>
        Task<LatestUploadDateBindingModel> GetUploadDateAsync();

        /// <summary>
        /// Асинхронный метод проверки существования даты загрузки.
        /// </summary>
        /// <param name="date">Дата зугрузки.</param>
        /// <returns>Была ли осуществлена загрузка в указанную дату.</returns>
        Task<bool> UploadDateExistsAsync(DateOnly date);

        /// <summary>
        /// Добавить дату загрузки в хранилище.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли добавить дату загрузки в хранилище.</returns>
        Task<bool> CreateUploadDateAsync(LatestUploadDateBindingModel uploadDate);

        /// <summary>
        /// Удалить дату загрузки из хранилища.
        /// </summary>
        /// <param name="uploadDate">Дата загрузки.</param>
        /// <returns>Удалось ли удалить дату загрузки из хранилища.</returns>
        Task<bool> DeleteUploadDateAsync(LatestUploadDateBindingModel uploadDate);

        /// <summary>
        /// Применить все изменения к хранилищу.
        /// </summary>
        /// <returns>Применились ли изменения к хранилищу.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Очистить хранилище от данных.
        /// </summary>
        /// <returns>Удалось ли очистить хранилище от дынных.</returns>
        Task<bool> Truncate();
    }
}
