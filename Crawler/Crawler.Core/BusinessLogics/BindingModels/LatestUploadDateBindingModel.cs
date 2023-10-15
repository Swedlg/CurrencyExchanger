namespace Crawler.Core.BusinessLogics.BindingModels
{
    /// <summary>
    /// Абстрактная модель дат последних загрузок.
    /// </summary>
    public abstract class LatestUploadDateBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Дата выгрузки информации о котировках валют.
        /// </summary>
        public virtual DateOnly UploadDate { get; set; }
    }
}
