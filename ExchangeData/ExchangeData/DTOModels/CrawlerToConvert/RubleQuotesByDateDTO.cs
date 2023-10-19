namespace ExchangeData.DTOModels.CrawlerToConvert
{
    /// <summary>
    /// Модель списка значений валют относительно рубля по дате.
    /// </summary>
    public class RubleQuotesByDateDTO
    {
        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Список значений валют относительно рубля.
        /// </summary>
        public List<RubleQuoteDTO> List { get; set; } = new List<RubleQuoteDTO>();
    };
}
