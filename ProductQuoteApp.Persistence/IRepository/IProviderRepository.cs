using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace ProductQuoteApp.Persistence
{
    public interface IProviderRepository : IDisposable
    {
        List<Provider> Providers();        
        IEnumerable<Provider> FindProvidersFilter(string filter, string sortBy);
        Task<List<Provider>> FindProvidersAsync();
        Task<Provider> FindProvidersByIDAsync(int providerID);
        Task CreateAsync(Provider providerToAdd);
        Task DeleteAsync(int providerID);
        Task UpdateAsync(Provider providerToSave);
    }
}
