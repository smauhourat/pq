using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class TransportTypeRepository : ITransportTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public TransportTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(TransportType transportTypeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.TransportTypes.Add(transportTypeToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "TransportTypeRepository.CreateAsync", timespan.Elapsed, "transportTypeToAdd={0}", transportTypeToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in TransportTypeRepository.CreateAsync(transportTypeToAdd={0})", transportTypeToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int transportTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                TransportType transportType = await db.TransportTypes.FindAsync(transportTypeID);
                db.TransportTypes.Remove(transportType);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "TransportTypeRepository.DeleteAsync", timespan.Elapsed, "transportTypeID={0}", transportTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in TransportTypeRepository.DeleteAsync(transportTypeID={0})", transportTypeID);
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

        public List<TransportType> FindTransportTypes()
        {
            var result = db.TransportTypes.ToList();
            return result;
        }

        public async Task<List<TransportType>> FindTransportTypesAsync()
        {
            var result = await db.TransportTypes.OrderBy(t => t.Description).ToListAsync();
            return result;
        }

        public async Task<TransportType> FindTransportTypesByIDAsync(int transportTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            TransportType transportType;
            try
            {
                transportType = await db.TransportTypes.FindAsync(transportTypeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "TransportTypeRepository.FindTransportTypesByIDAsync", timespan.Elapsed, "transportTypeID={0}", transportTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in TransportTypeRepository.FindTransportTypesByIDAsync(transportTypeID={0})", transportTypeID);
                throw;
            }

            return transportType;
        }

        public async Task UpdateAsync(TransportType transportTypeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(transportTypeToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "TransportTypeRepository.UpdateAsync", timespan.Elapsed, "transportTypeToSave={0}", transportTypeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in TransportTypeRepository.UpdateAsync(transportTypeToSave={0})", transportTypeToSave);
                throw;
            }
        }

        public List<TransportType> TransportTypesByGeographicArea(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.TransportTypes.OrderBy(t => t.Description).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.ProductsActive()", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.ProductsActive()");
                throw;
            }
        }
    }
}
