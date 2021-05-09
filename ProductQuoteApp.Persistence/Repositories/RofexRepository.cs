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
    public class RofexRepository : IRofexRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public RofexRepository(ILogger logger)
        {
            log = logger;
        }

        public List<Rofex> Rofexs()
        {
            var result = db.Rofexs.ToList();
            return result;
        }

        public async Task CreateAsync(Rofex rofexToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            if (db.Rofexs.Any(r => r.Days == rofexToAdd.Days))
            {
                throw new ValidationException("Ya existe una entrada Rofex para la cantidad de dias indicados.");
            }

            try
            {
                db.Rofexs.Add(rofexToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RofexRepository.CreateAsync", timespan.Elapsed, "rofexToAdd={0}", rofexToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RofexRepository.CreateAsync(rofexToAdd={0})", rofexToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int rofexID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Rofex rofex = await db.Rofexs.FindAsync(rofexID);
                db.Rofexs.Remove(rofex);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RofexRepository.DeleteAsync", timespan.Elapsed, "rofexID={0}", rofexID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RofexRepository.DeleteAsync(rofexID={0})", rofexID);
                throw;
            }
        }

        public async Task<List<Rofex>> FindRofexsAsync()
        {
            var result = await db.Rofexs.OrderBy(s=>s.Days).ToListAsync();
            return result;
        }

        public async Task UpdateAsync(Rofex rofexToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            if (db.Rofexs.Any(r => r.Days == rofexToSave.Days && r.RofexID != rofexToSave.RofexID))
            {
                throw new ValidationException("Ya existe una entrada Rofex para la cantidad de dias indicados.");
            }

            try
            {
                db.Entry(rofexToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "RofexRepository.UpdateAsync", timespan.Elapsed, "rofexToSave={0}", rofexToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in RofexRepository.UpdateAsync(rofexToSave={0})", rofexToSave);
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

        public async Task<Rofex> FindRofexByIDAsync(int rofexID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Rofex rofex;
            try
            {
                rofex = await db.Rofexs.FindAsync(rofexID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProviderRepository.FindRofexByIDAsync", timespan.Elapsed, "rofexID={0}", rofexID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProviderRepository.FindRofexByIDAsync(rofexID={0})", rofexID);
                throw;
            }

            return rofex;
        }
    }
}
