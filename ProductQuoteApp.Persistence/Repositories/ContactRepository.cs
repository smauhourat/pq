using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ContactRepository : IContactRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ContactRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task<List<Contact>> FindContactsByCustomerIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Contacts
                    .Where(t => t.CustomerID == customerID)
                    .OrderBy(t => t.DateContact).ToListAsync();

                if (result != null)
                {
                    foreach (Contact item in result)
                    {
                        db.Entry(item).Reference(x => x.ContactType).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactRepository.FindContactsByCustomerIDAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactRepository.FindContactsByCustomerIDAsync(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<Contact> FindContactByIDAsync(int contactID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Contact contact;
            try
            {
                contact = await db.Contacts.FindAsync(contactID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactRepository.FindContactByIDAsync", timespan.Elapsed, "contactID={0}", contactID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactRepository.FindContactByIDAsync(contactID={0})", contactID);
                throw;
            }

            return contact;
        }

        public async Task CreateAsync(Contact contact)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactRepository.CreateAsync", timespan.Elapsed, "contact={0}", contact);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactRepository.CreateAsync(contact={0})", contact);
                throw;
            }
        }

        public async Task UpdateAsync(Contact contact)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(contact).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactRepository.UpdateAsync", timespan.Elapsed, "contact={0}", contact);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactRepository.UpdateAsync(contact={0})", contact);
                throw;
            }
        }

        public async Task DeleteAsync(int contactID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Contact contact = await db.Contacts.FindAsync(contactID);
                db.Contacts.Remove(contact);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactRepository.DeleteAsync", timespan.Elapsed, "contactID={0}", contactID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactRepository.DeleteAsync(contactID={0})", contactID);
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
