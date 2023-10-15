using Storage.Core.BusinessLogics.BindingModels;

namespace Storage.Core.BusinessLogics.Interfaces
{
    public interface ICurrencyInfoRepository
    {
        Task<ICollection<CurrencyInfoBindingModel>> GetAllCurrencyInfoAsync();

        Task<CurrencyInfoBindingModel> GetCurrencyInfoAsync(string RId);

        Task<bool> CurrencyInfoExistsAsync(DateOnly date, string BaseRId, string RId);

        Task<bool> CreateCurrencyInfoDateAsync(CurrencyInfoBindingModel item);

        Task<bool> DeleteCurrencyInfoAsync(CurrencyInfoBindingModel item);

        Task<bool> SaveAsync();

        Task<bool> Truncate();
    }
}
