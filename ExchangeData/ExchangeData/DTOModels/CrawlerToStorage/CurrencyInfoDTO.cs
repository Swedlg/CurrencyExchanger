namespace ExchangeData.DTOModels.CrawlerToStorage
{
    /// <summary>
    /// Модель справочной информации о валюте.
    /// </summary>
    public class CurrencyInfoDTO
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="rID">RId.</param>
        /// <param name="name">Название валюты по русски.</param>
        /// <param name="engName">Название валюты по английски.</param>
        /// <param name="rId">Код родительской валюты.</param>
        /// <param name="iSOCharCode">Символьный код валюты.</param>
        public CurrencyInfoDTO(string rID, string name, string engName, string parentRId, string iSOCharCode)
        {
            RId = rID;
            Name = name;
            EngName = engName;
            ParentRId = parentRId;
            ISOCharCode = iSOCharCode;
        }

        /// <summary>
        /// RId.
        /// </summary>
        public string RId { get; set; }

        /// <summary>
        /// Название валюты по русски.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название валюты по английски.
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// Код родительской валюты.
        /// </summary>
        public string ParentRId { get; set; }

        /// <summary>
        /// Символьный код валюты.
        /// </summary>
        public string ISOCharCode { get; set; }
    }
}
