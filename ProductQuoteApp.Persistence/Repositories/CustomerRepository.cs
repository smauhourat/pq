using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CustomerRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(Customer customerToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Customers.Add(customerToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.CreateAsync", timespan.Elapsed, "customerToAdd={0}", customerToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.CreateAsync(customerToAdd={0})", customerToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int customerID)
        {
            Customer customer = null;
            Stopwatch timespan = Stopwatch.StartNew();

            if (db.ProductQuotes.Count(p => p.CustomerID == customerID) > 0)
            {
                throw new ValidationException("No puede eliminarse el Cliente porque tiene Cotizaciones relacionadas.");
            }

            try
            {
                customer = await db.Customers.FindAsync(customerID);                
                db.Customers.Remove(customer);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.DeleteAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.DeleteAsync(customerID={0})", customerID);
                throw;
            }
        }

        public async Task DeleteCascadeAsync(int customerID)
        {
            Customer customer = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ProductQuotes.RemoveRange(db.ProductQuotes.Where(p => p.CustomerID == customerID));
                db.SaveChanges();

                customer = await db.Customers.FindAsync(customerID);
                db.Customers.Remove(customer);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.DeleteAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.DeleteAsync(customerID={0})", customerID);
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

        public async Task<IEnumerable<Customer>> FindCustomersAsync()
        {
            var result = await db.Customers.ToListAsync();
            return result;
        }

        public IEnumerable<Customer> FindCustomers()
        {
            var result = db.Customers.ToList();
            return result;
        }

        public Customer FindCustomersByID(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Customer customer;
            try
            {
                customer = db.Customers.Find(customerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.FindCustomersByID", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.FindCustomersByID(customerID={0})", customerID);
                throw;
            }

            return customer;
        }

        public async Task<Customer> FindCustomersByIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Customer customer;
            try
            {
                customer = await db.Customers.FindAsync(customerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.FindCustomersByIDAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.FindCustomersByIDAsync(customerID={0})", customerID);
                throw;
            }

            return customer;
        }

        public async Task UpdateAsync(Customer customerToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(customerToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerRepository.UpdateAsync", timespan.Elapsed, "customerToSave={0}", customerToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerRepository.UpdateAsync(customerToSave={0})", customerToSave);
                throw;
            }
        }
    }
}
