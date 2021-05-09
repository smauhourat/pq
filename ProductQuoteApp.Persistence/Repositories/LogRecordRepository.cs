using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class LogRecordRepository : ILogRecordRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Create(LogRecord logRecordToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            db.LogRecords.Add(logRecordToAdd);
            db.SaveChanges();

            timespan.Stop();

        }

        public async Task CreateAsync(LogRecord logRecordToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            db.LogRecords.Add(logRecordToAdd);
            await db.SaveChangesAsync();

            timespan.Stop();
        }

        public async Task<LogRecord> FindLogRecordByIDAsync(int logRecordID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            LogRecord logRecord = await db.LogRecords.FindAsync(logRecordID);

            timespan.Stop();

            return logRecord;
        }

        public async Task DeleteAsync(int logRecordID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            LogRecord logRecord = await db.LogRecords.FindAsync(logRecordID);
            db.LogRecords.Remove(logRecord);
            await db.SaveChangesAsync();

            timespan.Stop();
        }

        public async Task<List<LogRecord>> FindLogRecordsAsync()
        {
            var result = await db.LogRecords.ToListAsync();
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
