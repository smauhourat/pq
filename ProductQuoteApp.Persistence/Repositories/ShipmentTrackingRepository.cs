using ProductQuoteApp.Logging;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ShipmentTrackingRepository : IShipmentTrackingRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ShipmentTrackingRepository(ILogger logger)
        {
            log = logger;
        }

        public ShipmentTracking FindShipmentTrackingByProductQuoteID(int productQuoteID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                ShipmentTracking shipmentTracking = db.ShipmentTrackings
                    //.Include(p => p.ProductQuote)
                    .Where(p => p.ProductQuoteID == productQuoteID).SingleOrDefault();

                timespan.Stop();
                log.TraceApi("SQL Database", "ShipmentTrackingRepository.FindShipmentTrackingByProductQuoteID", timespan.Elapsed, "productQuoteID={0}", productQuoteID);

                return shipmentTracking;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ShipmentTrackingRepository.FindShipmentTrackingByProductQuoteID(productQuoteID={0})", productQuoteID);
                throw;
            }
        }

        public void Create(ShipmentTracking shipmentTracking)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ShipmentTrackings.Add(shipmentTracking);
                db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ShipmentTrackingRepository.Create", timespan.Elapsed, "shipmentTracking={0}", shipmentTracking);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ShipmentTrackingRepository.Create(shipmentTracking={0})", shipmentTracking);
                throw;
            }
        }

        public async Task CreateAsync(ShipmentTracking shipmentTracking)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ShipmentTrackings.Add(shipmentTracking);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ShipmentTrackingRepository.CreateAsync", timespan.Elapsed, "shipmentTracking={0}", shipmentTracking);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ShipmentTrackingRepository.CreateAsync(shipmentTracking={0})", shipmentTracking);
                throw;
            }
        }

        public void Update(ShipmentTracking shipmentTracking)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(shipmentTracking).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "ShipmentTrackingRepository.Update", timespan.Elapsed, "shipmentTracking={0}", shipmentTracking);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ShipmentTrackingRepository.Update(shipmentTracking={0})", shipmentTracking);
                throw;
            }
        }

        public async Task UpdateAsync(ShipmentTracking shipmentTracking)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(shipmentTracking).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ShipmentTrackingRepository.UpdateAsync", timespan.Elapsed, "shipmentTracking={0}", shipmentTracking);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ShipmentTrackingRepository.UpdateAsync(shipmentTracking={0})", shipmentTracking);
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
