using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class FreightTypeRepository : IFreightTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public FreightTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(FreightType freightTypeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.FreightTypes.Add(freightTypeToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "FreightTypeRepository.CreateAsync", timespan.Elapsed, "freightTypeToAdd={0}", freightTypeToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in FreightTypeRepository.CreateAsync(freightTypeToAdd={0})", freightTypeToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int freightTypeID)
        {
            FreightType freightType = null;
            Stopwatch timespan = Stopwatch.StartNew();

            if (db.TransportTypes.Any(r => r.FreightTypeID == freightTypeID))
            {
                throw new ValidationException("Existen Tipos de Transportes asociados a este Tipo de Carga");
            }

            try
            {
                freightType = await db.FreightTypes.FindAsync(freightTypeID);
                db.FreightTypes.Remove(freightType);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "FreightTypeRepository.DeleteAsync", timespan.Elapsed, "freightTypeID={0}", freightTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in FreightTypeRepository.DeleteAsync(freightTypeID={0})", freightTypeID);
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

        public List<FreightType> FreightTypes()
        {
            var result = db.FreightTypes.OrderBy(f => f.Description).ToList();
            return result;
        }
        public async Task<List<FreightType>> FindFreightTypesAsync()
        {
            var result = await db.FreightTypes.ToListAsync();
            return result;
        }

        public FreightType FindFreightTypesByID(int freightTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            FreightType freightType;
            try
            {
                freightType = db.FreightTypes.Find(freightTypeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "FreightTypeRepository.FindFreightTypesByID", timespan.Elapsed, "freightTypeID={0}", freightTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in FreightTypeRepository.FindFreightTypesByID(freightTypeID={0})", freightTypeID);
                throw;
            }

            return freightType;
        }

        public async Task<FreightType> FindFreightTypesByIDAsync(int freightTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            FreightType freightType;
            try
            {
                freightType = await db.FreightTypes.FindAsync(freightTypeID);

                timespan.Stop();
                log.TraceApi("SQL Database", "FreightTypeRepository.FindFreightTypesByIDAsync", timespan.Elapsed, "freightTypeID={0}", freightTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in FreightTypeRepository.FindFreightTypesByIDAsync(freightTypeID={0})", freightTypeID);
                throw;
            }

            return freightType;
        }

        public async Task UpdateAsync(FreightType freightTypeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(freightTypeToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "FreightTypeRepository.UpdateAsync", timespan.Elapsed, "freightTypeToSave={0}", freightTypeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in FreightTypeRepository.UpdateAsync(freightTypeToSave={0})", freightTypeToSave);
                throw;
            }
        }

    }
}
