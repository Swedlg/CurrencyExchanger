namespace ExchangeData.DTOModels
{
    /// <summary>
    /// Справочная информация о валюте.
    /// </summary>
    public class CurrencyInfoDTO
    {
        public CurrencyInfoDTO(string id, string name, string engName, string rId, string iSOCharCode)
        {
            Id = id;
            Name = name;
            EngName = engName;
            RId = rId;
            ISOCharCode = iSOCharCode;
        }


        /// <summary>
        /// ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// Код родительской или базовой валюты.
        /// </summary>
        public string RId { get; set; }

        /// <summary>
        /// Код валюты.
        /// </summary>
        public string ISOCharCode { get; set; }
    }
}
