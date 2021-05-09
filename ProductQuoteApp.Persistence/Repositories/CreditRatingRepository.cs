using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CreditRatingRepository : ICreditRatingRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CreditRatingRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(CreditRating creditRatingToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CreditRatings.Add(creditRatingToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.CreateAsync", timespan.Elapsed, "creditRatingToAdd={0}", creditRatingToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.CreateAsync(creditRatingToAdd={0})", creditRatingToAdd);
                throw;
            }
        }

        public List<CreditRating> CreditRatings()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.CreditRatings.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.CreditRatings", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.CreditRatings()");
                throw;
            }
        }

        public async Task<List<CreditRating>> CreditRatingsAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CreditRatings.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.CreditRatingsAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.CreditRatingsAsync()");
                throw;
            }
        }

        public async Task DeleteAsync(int creditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CreditRating creditRating = await db.CreditRatings.FindAsync(creditRatingID);
                db.CreditRatings.Remove(creditRating);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.DeleteAsync", timespan.Elapsed, "creditRatingID={0}", creditRatingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.DeleteAsync(creditRatingID={0})", creditRatingID);
                throw;
            }
        }

        public async Task<CreditRating> FindCreditRatingByIDAsync(int creditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            CreditRating creditRating;
            try
            {
                creditRating = await db.CreditRatings.FindAsync(creditRatingID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.FindCreditRatingByIDAsync", timespan.Elapsed, "creditRatingID={0}", creditRatingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.FindCreditRatingByIDAsync(creditRatingID={0})", creditRatingID);
                throw;
            }

            return creditRating;
        }

        public async Task<List<CreditRating>> FindCreditRatingsAsync()
        {
            var result = await db.CreditRatings.ToListAsync();
            return result;
        }

        public async Task UpdateAsync(CreditRating creditRatingToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(creditRatingToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingRepository.UpdateAsync", timespan.Elapsed, "creditRatingToSave={0}", creditRatingToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingRepository.UpdateAsync(creditRatingToSave={0})", creditRatingToSave);
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
