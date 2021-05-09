using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class DueDateReasonRepository: IDueDateReasonRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public DueDateReasonRepository(ILogger logger)
        {
            log = logger;
        }

        public List<DueDateReason> DueDateReasons()
        {
            var result = db.DueDateReasons.OrderBy(p => p.Description).ToList();
            return result;
        }

        public IEnumerable<DueDateReason> FindDueDateReasonsFilter(string filter, string sortBy)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            filter = String.IsNullOrEmpty(filter) ? string.Empty : filter;
            try
            {
                var result = db.DueDateReasons
                    .Where(s => s.Description.Contains(filter))
                    .OrderBy(s => s.Description).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "DueDateReasonRepository.FindDueDateReasonsFilter", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DueDateReasonRepository.FindDueDateReasonsFilter()");
                throw;
            }
        }

        public async Task<List<DueDateReason>> FindDueDateReasonsAsync()
        {
            var result = await db.DueDateReasons.ToListAsync();
            return result;
        }

        public async Task<DueDateReason> FindDueDateReasonsByIDAsync(int dueDateReasonID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            DueDateReason dueDateReason;
            try
            {
                dueDateReason = await db.DueDateReasons.FindAsync(dueDateReasonID);

                timespan.Stop();
                log.TraceApi("SQL Database", "DueDateReasonRepository.DueDateReasonsByIDAsync", timespan.Elapsed, "dueDateReasonID={0}", dueDateReasonID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DueDateReasonRepository.DueDateReasonsByIDAsync(dueDateReasonID={0})", dueDateReasonID);
                throw;
            }

            return dueDateReason;
        }

        public async Task CreateAsync(DueDateReason dueDateReasonToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.DueDateReasons.Add(dueDateReasonToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DueDateReasonRepository.CreateAsync", timespan.Elapsed, "dueDateReasonToAdd={0}", dueDateReasonToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DueDateReasonRepository.CreateAsync(dueDateReasonToAdd={0})", dueDateReasonToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int dueDateReasonID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                DueDateReason dueDateReason = await db.DueDateReasons.FindAsync(dueDateReasonID);
                db.DueDateReasons.Remove(dueDateReason);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DueDateReasonRepository.DeleteAsync", timespan.Elapsed, "dueDateReasonID={0}", dueDateReasonID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DueDateReasonRepository.DeleteAsync(dueDateReasonID={0})", dueDateReasonID);
                throw;
            }
        }

        public async Task UpdateAsync(DueDateReason dueDateReasonToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(dueDateReasonToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "DueDateReasonRepository.UpdateAsync", timespan.Elapsed, "dueDateReasonToSave={0}", dueDateReasonToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in DueDateReasonRepository.UpdateAsync(dueDateReasonToSave={0})", dueDateReasonToSave);
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
