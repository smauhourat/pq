using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class PackagingRepository : IPackagingRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public PackagingRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(Packaging packagingToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Packagings.Add(packagingToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PackagingRepository.CreateAsync", timespan.Elapsed, "packagingToAdd={0}", packagingToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PackagingRepository.CreateAsync(packagingToAdd={0})", packagingToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int packagingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Packaging packaging = await db.Packagings.FindAsync(packagingID);
                db.Packagings.Remove(packaging);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PackagingRepository.DeleteAsync", timespan.Elapsed, "packagingID={0}", packagingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PackagingRepository.DeleteAsync(packagingID={0})", packagingID);
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

        public List<Packaging> Packagings()
        {
            var result = db.Packagings.OrderBy(p => p.Description).ToList();
            return result;
        }
        public async Task<List<Packaging>> FindPackagingsAsync()
        {
            var result = await db.Packagings.ToListAsync();
            return result;
        }

        public Packaging FindPackagingsByID(int packagingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Packaging packaging;
            try
            {
                packaging = db.Packagings.Find(packagingID);

                timespan.Stop();
                log.TraceApi("SQL Database", "PackagingRepository.FindPackagingsByID", timespan.Elapsed, "packagingID={0}", packagingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PackagingRepository.FindPackagingsByID(packagingID={0})", packagingID);
                throw;
            }

            return packaging;
        }

        public async Task<Packaging> FindPackagingsByIDAsync(int packagingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Packaging packaging;
            try
            {
                packaging = await db.Packagings.FindAsync(packagingID);

                timespan.Stop();
                log.TraceApi("SQL Database", "PackagingRepository.FindPackagingsByIDAsync", timespan.Elapsed, "packagingID={0}", packagingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PackagingRepository.FindPackagingsByIDAsync(packagingID={0})", packagingID);
                throw;
            }

            return packaging;
        }

        public async Task UpdateAsync(Packaging packagingToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(packagingToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PackagingRepository.UpdateAsync", timespan.Elapsed, "packagingToSave={0}", packagingToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PackagingRepository.UpdateAsync(packagingToSave={0})", packagingToSave);
                throw;
            }
        }

    }
}
