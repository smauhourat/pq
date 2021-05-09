using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class EmailAccountRepository : IEmailAccountRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public EmailAccountRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(EmailAccount emailAccountToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.EmailAccounts.Add(emailAccountToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmailAccountRepository.CreateAsync", timespan.Elapsed, "emailAccountToAdd={0}", emailAccountToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailAccountRepository.CreateAsync(emailAccountToAdd={0})", emailAccountToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int emailAccountID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                EmailAccount emailAccount = await db.EmailAccounts.FindAsync(emailAccountID);
                db.EmailAccounts.Remove(emailAccount);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmailAccountRepository.DeleteAsync", timespan.Elapsed, "emailAccountID={0}", emailAccountID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailAccountRepository.DeleteAsync(emailAccountID={0})", emailAccountID);
                throw;
            }
        }

        public async Task<List<EmailAccount>> FindEmailAccountsAsync()
        {
            var result = await db.EmailAccounts.ToListAsync();
            return result;
        }

        public EmailAccount FindEmailAccountsDefaultAsync()
        {
            EmailAccount emailAccount = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                emailAccount = db.EmailAccounts.Where(c => c.IsDefault).ToList().FirstOrDefault();

                timespan.Stop();
                log.TraceApi("SQL Database", "EmailAccountRepository.FindEmailAccountsDefaultAsync()", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailAccountRepository.FindEmailAccountsDefaultAsync()");
                throw;
            }

            return emailAccount;
        }

        public async Task<EmailAccount> FindEmailAccountsByIDAsync(int emailAccountID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            EmailAccount emailAccount;
            try
            {
                emailAccount = await db.EmailAccounts.FindAsync(emailAccountID);

                timespan.Stop();
                log.TraceApi("SQL Database", "EmailAccountRepository.FindEmailAccountsByIDAsync", timespan.Elapsed, "emailAccountID={0}", emailAccountID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailAccountRepository.FindEmailAccountsByIDAsync(emailAccountID={0})", emailAccountID);
                throw;
            }

            return emailAccount;
        }

        public async Task UpdateAsync(EmailAccount emailAccountToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(emailAccountToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();

                log.TraceApi("SQL Database", "EmailAccountRepository.UpdateAsync", timespan.Elapsed, "emailAccountToSave={0}", emailAccountToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in EmailAccountRepository.UpdateAsync(emailAccountToSave={0})", emailAccountToSave);
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
