using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CurrencyTypeRepository : ICurrencyTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CurrencyTypeRepository(ILogger logger)
        {
            log = logger;
        }
        public List<CurrencyType> CurrencyTypes()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.CurrencyTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "CurrencyTypeRepository.CurrencyTypes", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CurrencyTypeRepository.CurrencyTypes()");
                throw;
            }
        }

        public async Task<List<CurrencyType>> CurrencyTypesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CurrencyTypes.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CurrencyTypeRepository.CurrencyTypesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CurrencyTypeRepository.CurrencyTypesAsync()");
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
    }
}
