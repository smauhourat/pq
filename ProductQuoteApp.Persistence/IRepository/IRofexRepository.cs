using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IRofexRepository : IDisposable
    {
        List<Rofex> Rofexs();
        Task<List<Rofex>> FindRofexsAsync();
        Task<Rofex> FindRofexByIDAsync(int rofexID);
        Task CreateAsync(Rofex rofexToAdd);
        Task DeleteAsync(int rofexID);
        Task UpdateAsync(Rofex rofexToSave);

    }
}
