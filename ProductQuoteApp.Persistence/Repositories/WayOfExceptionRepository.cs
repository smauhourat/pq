using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class WayOfExceptionRepository : IWayOfExceptionRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public WayOfExceptionRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(WayOfException wayOfExceptionToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.WayOfExceptions.Add(wayOfExceptionToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "WayOfExceptionRepository.CreateAsync", timespan.Elapsed, "wayOfExceptionToAdd={0}", wayOfExceptionToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WayOfExceptionRepository.CreateAsync(wayOfExceptionToAdd={0})", wayOfExceptionToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int wayOfExceptionID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                WayOfException wayOfException = await db.WayOfExceptions.FindAsync(wayOfExceptionID);
                db.WayOfExceptions.Remove(wayOfException);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "WayOfExceptionRepository.DeleteAsync", timespan.Elapsed, "wayOfExceptionID={0}", wayOfExceptionID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WayOfExceptionRepository.DeleteAsync(wayOfExceptionID={0})", wayOfExceptionID);
                throw;
            }
        }

        public async Task<List<WayOfException>> FindWayOfExceptionsAsync()
        {
            var result = await db.WayOfExceptions.ToListAsync();
            return result;
        }

        public List<WayOfException> FindWayOfExceptions()
        {
            var result = db.WayOfExceptions.ToList();
            return result;
        }

        public async Task<WayOfException> FindWayOfExceptionsByIDAsync(int wayOfExceptionID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            WayOfException wayOfException;
            try
            {
                wayOfException = await db.WayOfExceptions.FindAsync(wayOfExceptionID);

                timespan.Stop();
                log.TraceApi("SQL Database", "WayOfExceptionRepository.FindProvidersByIDAsync", timespan.Elapsed, "wayOfExceptionID={0}", wayOfExceptionID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WayOfExceptionRepository.FindProvidersByIDAsync(wayOfExceptionID={0})", wayOfExceptionID);
                throw;
            }

            return wayOfException;
        }

        public IEnumerable<WayOfException> FindWayOfExceptionsFilter(string filter, string sortBy)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            filter = String.IsNullOrEmpty(filter) ? string.Empty : filter;
            try
            {
                var result = db.WayOfExceptions
                    .Where(s => s.Customer.Company.Contains(filter))
                    .OrderBy(s => s.Customer.Company).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "WayOfExceptionRepository.FindProvidersFilter", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WayOfExceptionRepository.FindProvidersFilter()");
                throw;
            }
        }

        public async Task UpdateAsync(WayOfException wayOfExceptionToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(wayOfExceptionToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "WayOfExceptionRepository.UpdateAsync", timespan.Elapsed, "wayOfExceptionToSave={0}", wayOfExceptionToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in WayOfExceptionRepository.UpdateAsync(wayOfExceptionToSave={0})", wayOfExceptionToSave);
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
