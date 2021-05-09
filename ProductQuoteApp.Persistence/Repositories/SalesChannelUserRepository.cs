using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SalesChannelUserRepository : ISalesChannelUserRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SalesChannelUserRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<SalesChannel>> FindSalesChannelsAvailableByUserAsync(string userID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<SalesChannelUser> salesChannelUsers = await db.SalesChannelUsers.Where(p => p.UserID == userID).ToListAsync();
                List<SalesChannel> salesChannels = await db.SalesChannels.ToListAsync();

                foreach (SalesChannelUser item in salesChannelUsers)
                {
                    if (salesChannels.Contains(item.SalesChannel))
                    {
                        salesChannels.Remove(item.SalesChannel);
                    }
                }


                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.FindSalesChannelsAvailableByUSerAsync", timespan.Elapsed, "userID={0}", userID);

                return salesChannels;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.FindSalesChannelsAvailableByUSerAsync(userID={0})", userID);
                throw;
            }
        }

        public async Task<List<SalesChannelUser>> FindSalesChannelsByUserIDAsync(string userID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.SalesChannelUsers
                    .Where(t => t.UserID == userID)
                    .OrderBy(t => t.SalesChannel.Code).ToListAsync();

                if (result != null)
                {
                    foreach (SalesChannelUser item in result)
                    {
                        db.Entry(item).Reference(x => x.SalesChannel).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.FindSalesChannelsByUserIDAsync", timespan.Elapsed, "userID={0}", userID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.FindSalesChannelsByUserIDAsync(userID={0})", userID);
                throw;
            }
        }

        public List<SalesChannelUser> FindSalesChannelsByUserID(string userID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.SalesChannelUsers
                    .Include(t => t.SalesChannel)
                    .Where(t => t.UserID == userID)
                    .OrderBy(t => t.SalesChannel.Code).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.FindSalesChannelsByUserID", timespan.Elapsed, "userID={0}", userID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.FindSalesChannelsByUserID(userID={0})", userID);
                throw;
            }
        }

        public async Task CreateAsync(SalesChannelUser salesChannelUserToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SalesChannelUsers.Add(salesChannelUserToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.CreateAsync", timespan.Elapsed, "salesChannelUserToAdd={0}", salesChannelUserToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.CreateAsync(salesChannelUserToAdd={0})", salesChannelUserToAdd);
                throw;
            }
        }

        public void Create(SalesChannelUser salesChannelUserToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SalesChannelUsers.Add(salesChannelUserToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.Create", timespan.Elapsed, "salesChannelUserToAdd={0}", salesChannelUserToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.Create(salesChannelUserToAdd={0})", salesChannelUserToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int salesChannelUserID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SalesChannelUser salesChannelUser = await db.SalesChannelUsers.FindAsync(salesChannelUserID);
                db.SalesChannelUsers.Remove(salesChannelUser);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.Delete", timespan.Elapsed, "salesChannelUserID={0}", salesChannelUserID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.Delete(salesChannelUserID={0})", salesChannelUserID);
                throw;
            }
        }

        public void Delete(int salesChannelUserID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SalesChannelUser salesChannelUser = db.SalesChannelUsers.Find(salesChannelUserID);
                db.SalesChannelUsers.Remove(salesChannelUser);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SalesChannelUserRepository.Delete", timespan.Elapsed, "salesChannelUserID={0}", salesChannelUserID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SalesChannelUserRepository.Delete(salesChannelUserID={0})", salesChannelUserID);
                throw;
            }
        }

        public async Task DeleteByUserAsync(string userID)
        {
            var salesChannelsUser = await this.FindSalesChannelsByUserIDAsync(userID);
            if (salesChannelsUser != null)
            {
                foreach (SalesChannelUser item in salesChannelsUser)
                {
                    await this.DeleteAsync(item.SalesChannelUserID);
                }
            }
        }

        public void DeleteByUser(string userID)
        {
            var salesChannelsUser = this.FindSalesChannelsByUserID(userID);
            if (salesChannelsUser != null)
            {
                foreach (SalesChannelUser item in salesChannelsUser)
                {
                    this.Delete(item.SalesChannelUserID);
                }
            }
        }

        public async Task AddAllSalesChannelsToUserAsync(string userID)
        {
            var salesChannels = await FindSalesChannelsAvailableByUserAsync(userID);

            foreach (SalesChannel salesChannel in salesChannels)
            {
                SalesChannelUser salesChannelUser = new SalesChannelUser
                {
                    SalesChannelID = salesChannel.SalesChannelID,
                    UserID = userID
                };

                await CreateAsync(salesChannelUser);
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
