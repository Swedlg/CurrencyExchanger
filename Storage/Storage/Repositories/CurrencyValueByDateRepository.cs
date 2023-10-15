using Storage.Core.BusinessLogics.BindingModels;
using Storage.Core.BusinessLogics.Interfaces;

namespace Storage.Repositories
{
    public class CurrencyValueByDateRepository : ICurrencyInfoByDateRepository
    {
        public Task<bool> CreateCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CurrencyValueByDateExistsAsync(DateOnly date, string BaseRId, string RId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item)
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyValueByDateBindingModel> GetCurrencyValueByDateAsync(DateOnly date, string BaseRId, string RId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<CurrencyValueByDateBindingModel>> GetFilteredAsync(DateOnly? dateFrom, DateOnly? dateTo, string BaseRId, string RId)
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
