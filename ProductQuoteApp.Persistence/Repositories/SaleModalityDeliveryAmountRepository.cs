using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityDeliveryAmountRepository : ISaleModalityDeliveryAmountRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityDeliveryAmountRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<DeliveryAmount>> FindDeliveryAmountsBySaleModalityAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModality saleModality = await db.SaleModalitys.Where(p => p.SaleModalityID == saleModalityID).SingleAsync();
                var result = saleModality.SaleModalityDeliveryAmounts.Select(s => s.DeliveryAmount).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityDeliveryAmountRepository.FindDeliveryAmountsBySaleModalityAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityDeliveryAmountRepository.FindDeliveryAmountsBySaleModalityAsync(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public List<DeliveryAmount> FindDeliveryAmountsBySaleModality(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModality saleModality = db.SaleModalitys.Where(p => p.SaleModalityID == saleModalityID).Single();
                var result = saleModality.SaleModalityDeliveryAmounts.Select(s => s.DeliveryAmount).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityDeliveryAmountRepository.FindDeliveryAmountsBySaleModality", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityDeliveryAmountRepository.FindDeliveryAmountsBySaleModality(saleModalityID={0})", saleModalityID);
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
