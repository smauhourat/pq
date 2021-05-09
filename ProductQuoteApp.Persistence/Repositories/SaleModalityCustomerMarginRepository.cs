using ProductQuoteApp.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityCustomerMarginRepository : ISaleModalityCustomerMarginRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityCustomerMarginRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(SaleModalityCustomerMargin saleModalityCustomerMarginToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityCustomerMargins.Add(saleModalityCustomerMarginToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCustomerMarginRepository.Create", timespan.Elapsed, "saleModalityCustomerMarginToAdd={0}", saleModalityCustomerMarginToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCustomerMarginRepository.Create(saleModalityCustomerMarginToAdd={0})", saleModalityCustomerMarginToAdd);
                throw;
            }
        }

        public async Task CreateAsync(SaleModalityCustomerMargin saleModalityCustomerMarginToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityCustomerMargins.Add(saleModalityCustomerMarginToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCustomerMarginRepository.CreateAsync", timespan.Elapsed, "saleModalityCustomerMarginToAdd={0}", saleModalityCustomerMarginToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCustomerMarginRepository.CreateAsync(saleModalityCustomerMarginToAdd={0})", saleModalityCustomerMarginToAdd);
                throw;
            }
        }


        public void DeleteByCustomer(int CustomerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityCustomerMargins.RemoveRange(db.SaleModalityCustomerMargins.Where(p => p.CustomerID == CustomerID));
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCustomerMarginRepository.DeleteByCustomer", timespan.Elapsed, "CustomerID={0}", CustomerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCustomerMarginRepository.DeleteByCustomer(CustomerID={0})", CustomerID);
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
