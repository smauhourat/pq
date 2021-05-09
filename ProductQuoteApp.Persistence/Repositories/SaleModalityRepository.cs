using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityRepository : ISaleModalityRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityRepository(ILogger logger)
        {
            log = logger;
        }
        public List<SaleModality> SaleModalitys()
        {
            var result = db.SaleModalitys.OrderBy(s => s.OrderView).ToList();
            return result;
        }

        public async Task<List<SaleModality>> FindSaleModalitysAsync()
        {
            var result = await db.SaleModalitys.OrderBy(s => s.OrderView).ToListAsync();
            return result;
        }

        public async Task<SaleModality> FindSaleModalityByIDAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SaleModality saleModality;
            try
            {
                saleModality = await db.SaleModalitys.FindAsync(saleModalityID);

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityRepository.FindSaleModalityByIDAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityRepository.FindSaleModalityByIDAsync(saleModalityID={0})", saleModalityID);
                throw;
            }

            return saleModality;
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
