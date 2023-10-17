namespace ExchangeData.DTOModels.ConvertToStorage
{
    /// <summary>
    /// DTO модель списка котировок пар валют по датам.
    /// </summary>
    public class CurrencyQuotesByDateListDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="list">Список котировок пар валют по датам.</param>
        public CurrencyQuotesByDateListDTO(List<CurrencyQuotesByDateDTO> list)
        {
            List = list;
        }

        /// <summary>
        /// Список котировок пар валют по датам.
        /// </summary>
        public List<CurrencyQuotesByDateDTO> List {  get; set; }
    }
}
