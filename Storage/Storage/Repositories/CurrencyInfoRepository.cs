using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;

namespace Storage.Repositories
{
    public class CurrencyInfoRepository : ICurrencyInfoRepository
    {
        public Task<bool> CreateCurrencyInfoDateAsync(CurrencyInfoBindingModel item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CurrencyInfoExistsAsync(DateOnly date, string BaseRId, string RId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCurrencyInfoAsync(CurrencyInfoBindingModel item)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<CurrencyInfoBindingModel>> GetAllCurrencyInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyInfoBindingModel> GetCurrencyInfoAsync(string RId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Truncate()
        {
            throw new NotImplementedException();
        }
    }
}
