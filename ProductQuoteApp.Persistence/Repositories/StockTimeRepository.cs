using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class StockTimeRepository : IStockTimeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public StockTimeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<StockTime> StockTimes()
        {
            var result = db.StockTimes.ToList();
            return result;
        }
        public async Task<List<StockTime>> FindStockTimesAsync()
        {
            var result = await db.StockTimes.ToListAsync();
            return result;
        }

        public StockTime FindStockTimesByID(int stockTimeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            StockTime stockTime;
            try
            {
                stockTime = db.StockTimes.Find(stockTimeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "StockTimeRepository.FindStockTimesByID", timespan.Elapsed, "stockTimeID={0}", stockTimeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in StockTimeRepository.FindStockTimesByID(stockTimeID={0})", stockTimeID);
                throw;
            }

            return stockTime;
        }

        public async Task<StockTime> FindStockTimesByIDAsync(int stockTimeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            StockTime stockTime;
            try
            {
                stockTime = await db.StockTimes.FindAsync(stockTimeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "StockTimeRepository.FindStockTimesByIDAsync", timespan.Elapsed, "stockTimeID={0}", stockTimeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in StockTimeRepository.FindStockTimesByIDAsync(stockTimeID={0})", stockTimeID);
                throw;
            }

            return stockTime;
        }

        public List<StockTime> FindStockTimesBySaleModalityID(int saleModalityID)
        {
            var result = db.SaleModalityStockTimes.Where(s => s.SaleModalityID == saleModalityID).Select(s => s.StockTime).ToList();

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
