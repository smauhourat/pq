using ProductQuoteApp.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityProductMarginRepository : ISaleModalityProductMarginRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityProductMarginRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(SaleModalityProductMargin saleModalityProductMarginToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProductMargins.Add(saleModalityProductMarginToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductMarginRepository.Create", timespan.Elapsed, "saleModalityProductMarginToAdd={0}", saleModalityProductMarginToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductMarginRepository.Create(saleModalityProductMarginToAdd={0})", saleModalityProductMarginToAdd);
                throw;
            }
        }

        public async Task CreateAsync(SaleModalityProductMargin saleModalityProductMarginToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProductMargins.Add(saleModalityProductMarginToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductMarginRepository.CreateAsync", timespan.Elapsed, "saleModalityProductMarginToAdd={0}", saleModalityProductMarginToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductMarginRepository.CreateAsync(saleModalityProductMarginToAdd={0})", saleModalityProductMarginToAdd);
                throw;
            }
        }


        public void DeleteByProduct(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProductMargins.RemoveRange(db.SaleModalityProductMargins.Where(p => p.ProductID == productID));
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductMarginRepository.DeleteByProduct", timespan.Elapsed, "productID={0}", productID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductMarginRepository.DeleteByProduct(productID={0})", productID);
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
