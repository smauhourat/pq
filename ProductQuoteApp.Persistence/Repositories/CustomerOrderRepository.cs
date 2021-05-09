using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CustomerOrderRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(CustomerOrder customerOrderToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                customerOrderToAdd.CustomerOrderStatusID = 1;
                db.CustomerOrders.Add(customerOrderToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.Create", timespan.Elapsed, "customerOrderToAdd={0}", customerOrderToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.Create(customerOrderToAdd={0})", customerOrderToAdd);
                throw;
            }
        }

        public void Approve(int customerOrderID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CustomerOrder customerOrder = this.FindCustomerOrdersByID(customerOrderID);
                customerOrder.CustomerOrderStatusID = 2;
                customerOrder.ApprovedDate = DateTime.Now;
                customerOrder.RejectedDate = null;
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.Approve", timespan.Elapsed, "customerOrderID={0}", customerOrderID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.Approve(customerOrderID={0})", customerOrderID);
                throw;
            }
        }

        public void Reject(int customerOrderID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CustomerOrder customerOrder = this.FindCustomerOrdersByID(customerOrderID);
                customerOrder.CustomerOrderStatusID = 3;
                customerOrder.RejectedDate = DateTime.Now;
                customerOrder.ApprovedDate = null;
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.Reject", timespan.Elapsed, "customerOrderID={0}", customerOrderID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.Reject(customerOrderID={0})", customerOrderID);
                throw;
            }
        }
        public async Task CreateAsync(CustomerOrder customerOrderToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                customerOrderToAdd.CustomerOrderStatusID = 1;
                db.CustomerOrders.Add(customerOrderToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.CreateAsync", timespan.Elapsed, "customerOrderToAdd={0}", customerOrderToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.CreateAsync(customerOrderToAdd={0})", customerOrderToAdd);
                throw;
            }
        }

        public List<CustomerOrder> CustomerOrders()
        {
            var result = db.CustomerOrders.ToList();
            return result;
        }

        public async Task<List<CustomerOrder>> CustomerOrdersAsync()
        {
            var result = await db.CustomerOrders.ToListAsync();
            return result;
        }

        public async Task DeleteAsync(int customerOrderID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CustomerOrder customerOrder = await db.CustomerOrders.FindAsync(customerOrderID);
                db.CustomerOrders.Remove(customerOrder);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.DeleteAsync", timespan.Elapsed, "customerOrderID={0}", customerOrderID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.DeleteAsync(customerOrderID={0})", customerOrderID);
                throw;
            }
        }

        public async Task<CustomerOrder> FindCustomerOrdersByIDAsync(int customerOrderID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            CustomerOrder customerOrder;
            try
            {
                customerOrder = await db.CustomerOrders.FindAsync(customerOrderID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.FindCustomerOrdersByIDAsync", timespan.Elapsed, "customerOrderID={0}", customerOrderID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.FindCustomerOrdersByIDAsync(customerOrderID={0})", customerOrderID);
                throw;
            }

            return customerOrder;
        }

        public CustomerOrder FindCustomerOrdersByID(int customerOrderID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            CustomerOrder customerOrder;
            try
            {
                customerOrder = db.CustomerOrders.Find(customerOrderID);

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.FindCustomerOrdersByID", timespan.Elapsed, "customerOrderID={0}", customerOrderID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.FindCustomerOrdersByID(customerOrderID={0})", customerOrderID);
                throw;
            }

            return customerOrder;
        }

        public async Task UpdateAsync(CustomerOrder customerOrderToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(customerOrderToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.UpdateAsync", timespan.Elapsed, "customerOrderToSave={0}", customerOrderToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.UpdateAsync(customerOrderToSave={0})", customerOrderToSave);
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

        public async Task<List<CustomerOrder>> FindCustomerOrdersByCustomerIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CustomerOrders
                    .Where(t => t.ProductQuote.CustomerID  == customerID)
                    .OrderByDescending(t => t.DateOrder).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerOrderRepository.FindCustomerOrdersByCustomerIDAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerOrderRepository.FindCustomerOrdersByCustomerIDAsync(customerID={0})", customerID);
                throw;
            }
        }
    }
}
