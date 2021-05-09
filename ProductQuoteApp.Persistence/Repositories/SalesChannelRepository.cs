using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SalesChannelRepository : ISalesChannelRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SalesChannelRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(SalesChannel salesChannelToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SalesChannels.Add(salesChannelToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.CreateAsync", timespan.Elapsed, "salesChannelToAdd={0}", salesChannelToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.CreateAsync(salesChannelToAdd={0})", salesChannelToAdd);
                throw;
            }
        }

        public List<SalesChannel> SalesChannels()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.SalesChannels.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.SalesChannels", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.SalesChannels()");
                throw;
            }
        }

        public async Task<List<SalesChannel>> SalesChannelsAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.SalesChannels.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.SalesChannelsAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.SalesChannelsAsync()");
                throw;
            }
        }

        public async Task DeleteAsync(int salesChannelID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SalesChannel salesChannel = await db.SalesChannels.FindAsync(salesChannelID);
                db.SalesChannels.Remove(salesChannel);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.DeleteAsync", timespan.Elapsed, "salesChannelID={0}", salesChannelID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.DeleteAsync(salesChannelID={0})", salesChannelID);
                throw;
            }
        }

        public async Task<SalesChannel> FindSalesChannelByIDAsync(int salesChannelID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SalesChannel salesChannel;
            try
            {
                salesChannel = await db.SalesChannels.FindAsync(salesChannelID);

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.FindSalesChannelByIDAsync", timespan.Elapsed, "salesChannelID={0}", salesChannelID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.FindSalesChannelByIDAsync(salesChannelID={0})", salesChannelID);
                throw;
            }

            return salesChannel;
        }

        public async Task<List<SalesChannel>> FindSalesChannelsAsync()
        {
            var result = await db.SalesChannels.ToListAsync();
            return result;
        }

        public List<SalesChannel> FindSalesChannels()
        {
            var result = db.SalesChannels.ToList();
            return result;
        }

        public async Task UpdateAsync(SalesChannel salesChannelToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(salesChannelToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelRepository.UpdateAsync", timespan.Elapsed, "salesChannelToSave={0}", salesChannelToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelRepository.UpdateAsync(salesChannelToSave={0})", salesChannelToSave);
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
