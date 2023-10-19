namespace Crawler.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Модель даты загрузки информации о валютах.
    /// </summary>
    public class UploadDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата зугрузки информации о валютах.
        /// </summary>
        public DateOnly UploadDate { get; set; }
    }
}
