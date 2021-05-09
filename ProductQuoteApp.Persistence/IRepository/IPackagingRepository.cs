using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IPackagingRepository : IDisposable
    {
        List<Packaging> Packagings();
        Task<List<Packaging>> FindPackagingsAsync();
        Packaging FindPackagingsByID(int packagingID);
        Task<Packaging> FindPackagingsByIDAsync(int packagingID);
        Task CreateAsync(Packaging packagingToAdd);
        Task DeleteAsync(int packagingID);
        Task UpdateAsync(Packaging packagingToSave);
    }
}
