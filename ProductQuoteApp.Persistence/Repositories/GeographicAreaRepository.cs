using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class GeographicAreaRepository : IGeographicAreaRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public GeographicAreaRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(GeographicArea geographicAreaToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.GeographicAreas.Add(geographicAreaToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaRepository.CreateAsync", timespan.Elapsed, "geographicAreaToAdd={0}", geographicAreaToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaRepository.CreateAsync(geographicAreaToAdd={0})", geographicAreaToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                GeographicArea geographicArea = await db.GeographicAreas.FindAsync(geographicAreaID);
                db.GeographicAreas.Remove(geographicArea);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaRepository.DeleteAsync", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaRepository.DeleteAsync(geographicAreaID={0})", geographicAreaID);
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

        public async Task<List<GeographicArea>> FindGeographicAreasAsync()
        {
            var result = await db.GeographicAreas.OrderBy(g => g.Name).ToListAsync();
            return result;
        }

        public async Task<GeographicArea> FindGeographicAreasByIDAsync(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            GeographicArea geographicArea;
            try
            {
                geographicArea = await db.GeographicAreas.FindAsync(geographicAreaID);

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaRepository.FindGeographicAreasByIDAsync", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaRepository.FindGeographicAreasByIDAsync(geographicAreaID={0})", geographicAreaID);
                throw;
            }

            return geographicArea;
        }

        public async Task UpdateAsync(GeographicArea geographicAreaToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(geographicAreaToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaRepository.UpdateAsync", timespan.Elapsed, "geographicAreaToSave={0}", geographicAreaToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaRepository.UpdateAsync(geographicAreaToSave={0})", geographicAreaToSave);
                throw;
            }
        }

        public List<GeographicArea> GeographicAreas()
        {
            var result = db.GeographicAreas.OrderBy(g => g.Name).ToList();
            return result;
        }

        public List<GeographicArea> FindGeographicAreasBySaleModalityID(int saleModalityID)
        {
            var result = db.SaleModalityGeographicAreas.Where(s => s.SaleModalityID == saleModalityID).Select(s => s.GeographicArea).ToList();

            return result;
        }
    }
}
