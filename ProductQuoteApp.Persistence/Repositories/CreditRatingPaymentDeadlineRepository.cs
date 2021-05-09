using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CreditRatingPaymentDeadlineRepository : ICreditRatingPaymentDeadlineRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CreditRatingPaymentDeadlineRepository(ILogger logger)
        {
            log = logger;
        }

        public List<PaymentDeadline> FindPaymentDeadlineByCreditRating(int creditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CreditRating creditRating = db.CreditRatings
                    .Where(p => p.CreditRatingID == creditRatingID).Single();
                var result = creditRating.CreditRatingPaymentDeadlines.Select(s => s.PaymentDeadline).OrderBy(s => s.Days).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.FindPaymentDeadlineByCreditRating", timespan.Elapsed, "creditRatingID={0}", creditRatingID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.FindPaymentDeadlineByCreditRating(creditRatingID={0})", creditRatingID);
                throw;
            }
        }

        public async Task<List<CreditRatingPaymentDeadline>> FindCreditRatingPaymentDeadlinesByCreditRatingAsync(int creditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CreditRatingPaymentDeadlines
                    .Where(t => t.CreditRatingID == creditRatingID)
                    .OrderByDescending(t => t.CreditRatingID).ToListAsync();

                if (result != null)
                {
                    foreach (CreditRatingPaymentDeadline item in result)
                    {
                        db.Entry(item).Reference(x => x.PaymentDeadline).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.FindCreditRatingPaymentDeadlinesByCreditRatingAsync", timespan.Elapsed, "creditRatingID={0}", creditRatingID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.FindCreditRatingPaymentDeadlinesByCreditRatingAsync(creditRatingID={0})", creditRatingID);
                throw;
            }
        }

        public List<PaymentDeadline> FindPaymentDeadlineAvailableByCreditRating(int creditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<CreditRatingPaymentDeadline> creditRatingPaymentDeadlines = db.CreditRatingPaymentDeadlines.Where(p => p.CreditRatingID == creditRatingID).ToList();
                List<PaymentDeadline> paymentDeadlines = db.PaymentDeadlines.ToList();

                foreach (CreditRatingPaymentDeadline item in creditRatingPaymentDeadlines)
                {
                    if (paymentDeadlines.Contains(item.PaymentDeadline))
                    {
                        paymentDeadlines.Remove(item.PaymentDeadline);
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.FindPaymentDeadlineAvailableByCreditRating", timespan.Elapsed, "creditRatingID={0}", creditRatingID);

                return paymentDeadlines;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.FindPaymentDeadlineAvailableByCreditRating(creditRatingID={0})", creditRatingID);
                throw;
            }
        }

        public void Create(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CreditRatingPaymentDeadlines.Add(creditRatingPaymentDeadlineToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.Create", timespan.Elapsed, "creditRatingPaymentDeadlineToAdd={0}", creditRatingPaymentDeadlineToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.Create(creditRatingPaymentDeadlineToAdd={0})", creditRatingPaymentDeadlineToAdd);
                throw;
            }
        }

        public Task CreateAsync(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToAdd)
        {
            throw new NotImplementedException();
        }

        public void Delete(int creditRatingPaymentDeadlineID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CreditRatingPaymentDeadline crpd = db.CreditRatingPaymentDeadlines.Find(creditRatingPaymentDeadlineID);
                db.CreditRatingPaymentDeadlines.Remove(crpd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.Delete", timespan.Elapsed, "creditRatingPaymentDeadlineID={0}", creditRatingPaymentDeadlineID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.Delete(creditRatingPaymentDeadlineID={0})", creditRatingPaymentDeadlineID);
                throw;
            }
        }

        public Task DeleteAsync(int creditRatingPaymentDeadlineID)
        {
            throw new NotImplementedException();
        }

        public void Update(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(creditRatingPaymentDeadlineToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CreditRatingPaymentDeadlineRepository.Update", timespan.Elapsed, "creditRatingPaymentDeadlineToSave={0}", creditRatingPaymentDeadlineToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CreditRatingPaymentDeadlineRepository.Update(creditRatingPaymentDeadlineToSave={0})", creditRatingPaymentDeadlineToSave);
                throw;
            }
        }

        public Task UpdateAsync(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToSave)
        {
            throw new NotImplementedException();
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
