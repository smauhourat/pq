using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class DeliveryAmountRepository : IDeliveryAmountRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public DeliveryAmountRepository(ILogger logger)
        {
            log = logger;
        }

        public List<DeliveryAmount> DeliveryAmounts()
        {
            var result = db.DeliveryAmounts.ToList();
            return result;
        }
        public async Task<List<DeliveryAmount>> FindDeliveryAmountsAsync()
        {
            var result = await db.DeliveryAmounts.ToListAsync();
            return result;
        }

        public DeliveryAmount FindDeliveryAmountsByID(int deliveryAmountID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            DeliveryAmount deliveryAmount;
            try
            {
                deliveryAmount = db.DeliveryAmounts.Find(deliveryAmountID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DeliveryAmountRepository.FindDeliveryAmountsByID", timespan.Elapsed, "deliveryAmountID={0}", deliveryAmountID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DeliveryAmountRepository.FindDeliveryAmountsByID(deliveryAmountID={0})", deliveryAmountID);
                throw;
            }

            return deliveryAmount;
        }

        public async Task<DeliveryAmount> FindDeliveryAmountsByIDAsync(int deliveryAmountID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            DeliveryAmount deliveryAmount;
            try
            {
                deliveryAmount = await db.DeliveryAmounts.FindAsync(deliveryAmountID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DeliveryAmountRepository.FindDeliveryAmountsByIDAsync", timespan.Elapsed, "deliveryAmountID={0}", deliveryAmountID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DeliveryAmountRepository.FindDeliveryAmountsByIDAsync(deliveryAmountID={0})", deliveryAmountID);
                throw;
            }

            return deliveryAmount;
        }

        public List<DeliveryAmount> FindDeliveryAmountsBySaleModalityID(int saleModalityID)
        {
            var result = db.SaleModalityDeliveryAmounts.Where(s => s.SaleModalityID == saleModalityID).Select(s => s.DeliveryAmount).ToList();

            return result;
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
