using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ProviderRepository : IProviderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ProviderRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(Provider providerToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Providers.Add(providerToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProviderRepository.CreateAsync", timespan.Elapsed, "providerToAdd={0}", providerToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.CreateAsync(providerToAdd={0})", providerToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int providerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Provider provider = await db.Providers.FindAsync(providerID);
                db.Providers.Remove(provider);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProviderRepository.DeleteAsync", timespan.Elapsed, "providerID={0}", providerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.DeleteAsync(providerID={0})", providerID);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
                db = null;
            }
        }

        public List<Provider> Providers()
        {
            var result = db.Providers.OrderBy(p => p.ProviderName).ToList();
            return result;
        }

        public IEnumerable<Provider> FindProvidersFilter(string filter, string sortBy)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            filter = String.IsNullOrEmpty(filter) ? string.Empty : filter;
            try
            {
                if (filter == "throw")
                    throw new Exception("Este es un error de prueba");

                var result = db.Providers
                    .Where(s => s.ProviderName.Contains(filter))
                    .OrderBy(s => s.ProviderName).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProviderRepository.FindProvidersFilter", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.FindProvidersFilter()");
                throw;
            }
        }

        public async Task<List<Provider>> FindProvidersAsync()
        {
            var result = await db.Providers.ToListAsync();
            return result;
        }

        public async Task<Provider> FindProvidersByIDAsync(int providerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Provider provider;
            try
            {
                provider = await db.Providers.FindAsync(providerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProviderRepository.FindProvidersByIDAsync", timespan.Elapsed, "providerID={0}", providerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.FindProvidersByIDAsync(providerID={0})", providerID);
                throw;
            }

            return provider;
        }

        public async Task UpdateAsync(Provider providerToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(providerToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "ProviderRepository.UpdateAsync", timespan.Elapsed, "providerToSave={0}", providerToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.UpdateAsync(providerToSave={0})", providerToSave);
                throw;
            }
        }
    }
}
