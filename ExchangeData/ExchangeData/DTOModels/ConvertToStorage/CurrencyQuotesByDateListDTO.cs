namespace ExchangeData.DTOModels.ConvertToStorage
{
    /// <summary>
    /// DTO модель списка котировок пар валют по датам.
    /// </summary>
    public class CurrencyQuotesByDateListDTO
    {
        /// <summary>
        /// Список котировок пар валют по датам.
        /// </summary>
        public List<CurrencyQuotesByDateDTO> List {  get; set; } = new List<CurrencyQuotesByDateDTO>();
    }
}
