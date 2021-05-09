using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IWayOfExceptionRepository : IDisposable
    {
        IEnumerable<WayOfException> FindWayOfExceptionsFilter(string filter, string sortBy);
        Task<List<WayOfException>> FindWayOfExceptionsAsync();
        List<WayOfException> FindWayOfExceptions();
        Task<WayOfException> FindWayOfExceptionsByIDAsync(int wayOfExceptionID);
        Task CreateAsync(WayOfException wayOfExceptionToAdd);
        Task DeleteAsync(int wayOfExceptionID);
        Task UpdateAsync(WayOfException wayOfExceptionToSave);
    }
}
