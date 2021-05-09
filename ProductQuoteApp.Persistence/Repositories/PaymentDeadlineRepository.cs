using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class PaymentDeadlineRepository : IPaymentDeadlineRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public PaymentDeadlineRepository(ILogger logger)
        {
            log = logger;
        }
        public List<PaymentDeadline> PaymentDeadlines()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.PaymentDeadlines.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.PaymentDeadlines", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.PaymentDeadlines()");
                throw;
            }
        }

        public async Task<List<PaymentDeadline>> PaymentDeadlinesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.PaymentDeadlines.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.PaymentDeadlinesAsync", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.PaymentDeadlinesAsync()");
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

        public async Task<List<PaymentDeadline>> FindPaymentDeadlinesAsync()
        {
            var result = await db.PaymentDeadlines.OrderBy(p => p.Months).ToListAsync();
            return result;
        }

        public PaymentDeadline FindPaymentDeadlineByID(int paymentDeadlineID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            PaymentDeadline paymentDeadline;
            try
            {
                paymentDeadline = db.PaymentDeadlines.Find(paymentDeadlineID);

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.FindPaymentDeadlineByID", timespan.Elapsed, "paymentDeadlineID={0}", paymentDeadlineID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.FindPaymentDeadlineByID(paymentDeadlineID={0})", paymentDeadlineID);
                throw;
            }

            return paymentDeadline;
        }

        public async Task<PaymentDeadline> FindPaymentDeadlineByIDAsync(int paymentDeadlineID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            PaymentDeadline paymentDeadline;
            try
            {
                paymentDeadline = await db.PaymentDeadlines.FindAsync(paymentDeadlineID);

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.FindPaymentDeadlineByIDAsync", timespan.Elapsed, "paymentDeadlineID={0}", paymentDeadlineID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.FindPaymentDeadlineByIDAsync(paymentDeadlineID={0})", paymentDeadlineID);
                throw;
            }

            return paymentDeadline;
        }

        public async Task CreateAsync(PaymentDeadline paymentDeadlineToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.PaymentDeadlines.Add(paymentDeadlineToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.CreateAsync", timespan.Elapsed, "paymentDeadlineToAdd={0}", paymentDeadlineToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.CreateAsync(paymentDeadlineToAdd={0})", paymentDeadlineToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int paymentDeadlineID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                PaymentDeadline paymentDeadline = await db.PaymentDeadlines.FindAsync(paymentDeadlineID);
                db.PaymentDeadlines.Remove(paymentDeadline);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.DeleteAsync", timespan.Elapsed, "paymentDeadlineID={0}", paymentDeadlineID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.DeleteAsync(paymentDeadlineID={0})", paymentDeadlineID);
                throw;
            }
        }

        public async Task UpdateAsync(PaymentDeadline paymentDeadlineToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(paymentDeadlineToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PaymentDeadlineRepository.UpdateAsync", timespan.Elapsed, "paymentDeadlineToSave={0}", paymentDeadlineToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PaymentDeadlineRepository.UpdateAsync(paymentDeadlineToSave={0})", paymentDeadlineToSave);
                throw;
            }
        }
    }
}
