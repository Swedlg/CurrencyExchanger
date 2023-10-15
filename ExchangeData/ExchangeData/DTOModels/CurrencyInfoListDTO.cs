namespace ExchangeData.DTOModels
{
    /// <summary>
    /// Запись списка справочной информации по валютам.
    /// </summary>
    /// <param name="list">Список справочной информации по валютам.</param>
    public record CurrencyInfoListDTO(List<CurrencyInfoDTO> list);
}
