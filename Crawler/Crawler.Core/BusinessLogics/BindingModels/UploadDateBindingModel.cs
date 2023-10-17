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
        public virtual int Id { get; set; }

        /// <summary>
        /// Дата зугрузки информации о валютах.
        /// </summary>
        public virtual DateOnly UploadDate { get; set; }
    }
}
