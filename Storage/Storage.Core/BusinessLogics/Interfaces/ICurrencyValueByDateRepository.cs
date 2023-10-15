using Storage.Core.BusinessLogics.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Core.BusinessLogics.Interfaces
{
    public interface ICurrencyValueByDateRepository
    {
        
        Task<ICollection<CurrencyValueByDateBindingModel>> GetFilteredAsync(
            DateOnly? dateFrom,
            DateOnly? dateTo,
            string BaseRId,
            string RId);

        
        Task<CurrencyValueByDateBindingModel> GetCurrencyValueByDateAsync(DateOnly date, string BaseRId, string RId);

        
        Task<bool> CurrencyValueByDateExistsAsync(DateOnly date, string BaseRId, string RId);

        
        Task<bool> CreateCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item);

        
        Task<bool> DeleteCurrencyValueByDateAsync(CurrencyValueByDateBindingModel item);

        
        Task<bool> SaveAsync();

        
        Task<bool> Truncate();
    }
}
