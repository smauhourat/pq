using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ProductQuoteApp.Persistence
{
    public class ProductQuoteRepository : IProductQuoteRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ProductQuoteRepository(ILogger logger)
        {
            log = logger;
        }

        //Llamado por SELLERUSER
        public async Task<List<ProductQuote>> FindProductQuotesByUserIDAsync(string ownerUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            
            try
            {
                var result = await db.ProductQuotes
                    .Include(u => u.User)
                    .Include(a => a.CustomerOrder)
                    .Include(b => b.DueDateReason)
                    .Include(c => c.ShipmentTracking)
                    .Where(t => string.IsNullOrEmpty(ownerUserId) || t.UserId == ownerUserId)
                    .Where(u => string.IsNullOrEmpty(search)
                        || u.UserFullName.ToLower().Contains(search.ToLower())
                        || u.ProductQuoteCode.ToLower().Contains(search.ToLower())
                        || u.CustomerCompany.ToLower().Contains(search.ToLower())
                        || u.ProductName.ToLower().Contains(search.ToLower())
                        || u.ProductProviderName.ToLower().Contains(search.ToLower())
                     )
                    .Where(d => dateFrom == default(DateTime) ||
                        (
                            (isQuote && DbFunctions.TruncateTime(d.DateQuote) >= dateFrom)
                            ||
                            (!isQuote && d.CustomerOrder.DateOrder >= dateFrom)
                        )
                     )
                    .Where(d => dateTo == default(DateTime) ||
                        (
                            (isQuote && DbFunctions.TruncateTime(d.DateQuote) <= dateTo)
                            ||
                            (!isQuote && d.CustomerOrder.DateOrder <= dateTo)
                        )
                     )
                     .Where(q => (isQuote && q.CustomerOrder == null) || (!isQuote && q.CustomerOrder != null))
                    .OrderByDescending(t => t.DateQuote)
                    .ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByUserIDAsync", timespan.Elapsed, "ownerUserId={0}", ownerUserId);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByUserIDAsync(ownerUserId={0})", ownerUserId);
                throw;
            }
        }

        public async Task<List<ProductQuote>> FindProductQuotesByCustomerAndUserIDAsync(int customerID, string userId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.ProductQuotes
                    .Include(u => u.User)
                    .Include(a => a.CustomerOrder)
                    .Include(b => b.DueDateReason)
                    .Include(c => c.ShipmentTracking)
                    .Where(t => t.CustomerID == (customerID == -1 ? t.CustomerID : customerID))
                    .Where(t => t.UserId == userId || t.IsSellerUser)
                    .Where(u => string.IsNullOrEmpty(search)
                        || u.UserFullName.ToLower().Contains(search.ToLower())
                        || u.ProductQuoteCode.ToLower().Contains(search.ToLower())
                        || u.CustomerCompany.ToLower().Contains(search.ToLower())
                        || u.ProductName.ToLower().Contains(search.ToLower())
                        || u.ProductProviderName.ToLower().Contains(search.ToLower())
                     )
                    .Where(d => dateFrom == default(DateTime) ||
                        (
                            (isQuote && DbFunctions.TruncateTime(d.DateQuote) >= dateFrom)
                            ||
                            (!isQuote && d.CustomerOrder.DateOrder >= dateFrom)
                        )
                     )
                    .Where(d => dateTo == default(DateTime) ||
                        (
                            (isQuote && DbFunctions.TruncateTime(d.DateQuote) <= dateTo)
                            ||
                            (!isQuote && d.CustomerOrder.DateOrder <= dateTo)
                        )
                     )
                     .Where(q => (isQuote && q.CustomerOrder == null) || (!isQuote && q.CustomerOrder != null))
                    .OrderByDescending(t => t.DateQuote).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByCustomerAndUserIDAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByCustomerAndUserIDAsync(customerID={0})", customerID);
                throw;
            }
        }


        public ProductQuote FindProductQuotesByID(int productQuoteID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            ProductQuote productQuote;
            try
            {
                productQuote = db.ProductQuotes.Find(productQuoteID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByID", timespan.Elapsed, "productQuoteID={0}", productQuoteID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByID(productQuoteID={0})", productQuoteID);
                throw;
            }

            return productQuote;
        }


        public async Task<ProductQuote> FindProductQuotesByIDAsync(int productQuoteID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            ProductQuote productQuote;
            try
            {
                productQuote = await db.ProductQuotes.FindAsync(productQuoteID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByIDAsync", timespan.Elapsed, "productQuoteID={0}", productQuoteID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByIDAsync(productQuoteID={0})", productQuoteID);
                throw;
            }

            return productQuote;
        }

        public async Task<ProductQuote> FindProductQuotesByIDUserIDAsync(int productQuoteID, string userId)
        {
            ProductQuote productQuote = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                productQuote = await db.ProductQuotes
                    .Where(t => t.ProductQuoteID == productQuoteID)
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.DateQuote).SingleOrDefaultAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByIDUserIDAsync", timespan.Elapsed, "productQuoteID={0}", productQuoteID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByIDUserIDAsync(productQuoteID={0})", productQuoteID);
                throw;
            }

            return productQuote;
        }

        public async Task<ProductQuote> FindProductQuotesByIDCustomerAndUserIDAsync(int productQuoteID, int customerID, string userId)
        {
            ProductQuote productQuote = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                productQuote = await db.ProductQuotes
                    .Where(t => t.ProductQuoteID == productQuoteID)
                    .Where(t => t.CustomerID == (customerID == -1 ? t.CustomerID : customerID))
                    .Where(t => t.UserId == userId || t.IsSellerUser)
                    .OrderByDescending(t => t.DateQuote).SingleOrDefaultAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.FindProductQuotesByIDUserIDAsync", timespan.Elapsed, "productQuoteID={0}", productQuoteID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.FindProductQuotesByIDUserIDAsync(productQuoteID={0})", productQuoteID);
                throw;
            }

            return productQuote;
        }


        private bool HasCustomerOrder(int productQuoteID)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Where(o => o.ProductQuoteID == productQuoteID).FirstOrDefault();
            return (customerOrder != null);
        }

        public void Create(ProductQuote productQuoteToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                productQuoteToAdd.DateQuote = DateTime.Now;
                db.ProductQuotes.Add(productQuoteToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.CreateAsync", timespan.Elapsed, "productQuoteToAdd={0}", productQuoteToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.CreateAsync(productQuoteToAdd={0})", productQuoteToAdd);
                throw;
            }
        }

        public async Task CreateAsync(ProductQuote productQuoteToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                productQuoteToAdd.DateQuote = DateTime.Now;
                db.ProductQuotes.Add(productQuoteToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.CreateAsync", timespan.Elapsed, "productQuoteToAdd={0}", productQuoteToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.CreateAsync(productQuoteToAdd={0})", productQuoteToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int productQuoteID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                if (HasCustomerOrder(productQuoteID))
                    throw new ProductQuoteAppException("La Cotizacion no puede ser eliminada porque tiene una OC asociada.");

                ProductQuote productQuote = await db.ProductQuotes.FindAsync(productQuoteID);
                db.ProductQuotes.Remove(productQuote);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.DeleteAsync", timespan.Elapsed, "productQuoteID={0}", productQuoteID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.DeleteAsync(productQuoteID={0})", productQuoteID);
                throw;
            }
        }


        public void UpdatePdf(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(productQuoteToSave).Property(p => p.ProductQuotePDF).IsModified = true;
                db.Entry(productQuoteToSave).Property(p => p.ProductQuoteSmallPDF).IsModified = true;
                db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.UpdatePdf", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.UpdatePdf(productQuoteToSave={0})", productQuoteToSave);
                throw;
            }
        }

        public async Task UpdateDueDateReasonAsync(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ProductQuotes.Attach(productQuoteToSave);
                db.Entry(productQuoteToSave).Property(p => p.DueDateReasonID).IsModified = true;
                db.Entry(productQuoteToSave).Property(p => p.Observations).IsModified = true;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.UpdateDueDateReasonAsync", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.UpdateDueDateReasonAsync(productQuoteToSave={0})", productQuoteToSave);
                throw;
            }
        }

        public async Task UpdateReasonsForClosureAsync(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ProductQuotes.Attach(productQuoteToSave);
                db.Entry(productQuoteToSave).Property(p => p.ReasonsForClosureID).IsModified = true;
                db.Entry(productQuoteToSave).Property(p => p.ClosureDate).IsModified = true;
                db.Entry(productQuoteToSave).Property(p => p.ClosureObservations).IsModified = true;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.UpdateReasonsForClosureAsync", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.UpdateReasonsForClosureAsync(productQuoteToSave={0})", productQuoteToSave);
                throw;
            }
        }

        public async Task UpdateDateSentAsync(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ProductQuotes.Attach(productQuoteToSave);
                db.Entry(productQuoteToSave).Property(p => p.DateSent).IsModified = true;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.UpdateDateSentAsync", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.UpdateDateSentAsync(productQuoteToSave={0})", productQuoteToSave);
                throw;
            }
        }


        public void Update(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(productQuoteToSave).State = EntityState.Modified;
                db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.Update", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.Update(productQuoteToSave={0})", productQuoteToSave);
                throw;
            }
        }

        public async Task UpdateAsync(ProductQuote productQuoteToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(productQuoteToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductQuoteRepository.UpdateAsync", timespan.Elapsed, "productQuoteToSave={0}", productQuoteToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteRepository.UpdateAsync(productQuoteToSave={0})", productQuoteToSave);
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
