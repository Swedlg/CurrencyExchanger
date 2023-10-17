namespace ExchangeData.DTOModels.CrawlerToConvert
{
    /// <summary>
    /// Модель списка значений валют относительно рубля по дате.
    /// </summary>
    public class RubleQuotesByDateDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <param name="list">Список значений валют относительно рубля.</param>
        public RubleQuotesByDateDTO(DateTime date, List<RubleQuoteDTO> list)
        {
            Date = date;
            List = list;
        }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Список значений валют относительно рубля.
        /// </summary>
        public List<RubleQuoteDTO> List { get; set; }
    };
}
