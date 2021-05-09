using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityGeographicAreaRepository : ISaleModalityGeographicAreaRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityGeographicAreaRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<GeographicArea>> FindGeographicAreasBySaleModalityAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModality saleModality = await db.SaleModalitys.Where(p => p.SaleModalityID == saleModalityID).SingleAsync();
                var result = saleModality.SaleModalityGeographicAreas.Select(s => s.GeographicArea).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityGeographicAreaRepository.FindGeographicAreasBySaleModalityAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityGeographicAreaRepository.FindGeographicAreasBySaleModalityAsync(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public List<GeographicArea> FindGeographicAreasBySaleModality(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModality saleModality = db.SaleModalitys.Where(p => p.SaleModalityID == saleModalityID).Single();
                var result = saleModality.SaleModalityGeographicAreas.Select(s => s.GeographicArea).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityGeographicAreaRepository.FindGeographicAreasBySaleModality", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityGeographicAreaRepository.FindGeographicAreasBySaleModality(saleModalityID={0})", saleModalityID);
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
