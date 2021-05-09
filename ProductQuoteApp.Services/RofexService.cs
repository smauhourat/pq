using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Services
{
    public class RofexService : IRofexService 
    {
        private IRofexRepository rofexRepository = null;

        public RofexService(IRofexRepository rofexRepo)
        {
            rofexRepository = rofexRepo;
        }

        public List<Rofex> Rofexs()
        {
            return rofexRepository.Rofexs();
        }
        public async Task<List<Rofex>> FindRofexsAsync()
        {
            return await rofexRepository.FindRofexsAsync();
        }

        public async Task<Rofex> FindRofexByIDAsync(int rofexID)
        {
            return await rofexRepository.FindRofexByIDAsync(rofexID);
        }

        public async Task CreateAsync(Rofex rofexToAdd)
        {
            //throw new ValidationException("No puede eliminarse el Cliente porque tiene Cotizaciones relacionadas.");

            await rofexRepository.CreateAsync(rofexToAdd);
        }

        public async Task DeleteAsync(int rofexID)
        {
            await rofexRepository.DeleteAsync(rofexID);
        }

        public async Task UpdateAsync(Rofex rofexToSave)
        {
            await rofexRepository.UpdateAsync(rofexToSave);
        }

        public bool ExistRofex(Rofex rofex)
        {
            return false;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && rofexRepository != null)
            {
                rofexRepository.Dispose();
                rofexRepository = null;
            }
        }
    }
}
