namespace ExchangeData.DTOModels
{
    /// <summary>
    /// Запись списка информации о валютных котировках по датам.
    /// </summary>
    /// <param name="list">Список информации о валютных котировках по датам.</param>
    public record CurrencyValuesByDatesListDTO(List<CurrencyValueByDateListDTO> list);
}
