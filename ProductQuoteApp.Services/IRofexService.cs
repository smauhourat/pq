using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Services
{
    public interface IRofexService : IDisposable
    {
        List<Rofex> Rofexs();
        Task<List<Rofex>> FindRofexsAsync();
        Task<Rofex> FindRofexByIDAsync(int rofexID);
        Task CreateAsync(Rofex rofexToAdd);
        Task DeleteAsync(int rofexID);
        Task UpdateAsync(Rofex rofexToSave);
        bool ExistRofex(Rofex rofex);
    }
}
