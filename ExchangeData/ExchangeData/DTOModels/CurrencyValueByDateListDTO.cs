namespace ExchangeData.DTOModels
{
    /// <summary>
    /// Запись списка информации по котировкам валют по датам.
    /// </summary>
    /// <param name="date">Дата получения валютных котировок</param>
    /// <param name="list">Список информации по котировкам валют по датам</param>
    public record CurrencyValueByDateListDTO(DateOnly date, List<CurrencyValueByDateDTO> list);
}
