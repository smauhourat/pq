using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityCreditRatingRepository : ISaleModalityCreditRatingRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityCreditRatingRepository(ILogger logger)
        {
            log = logger;
        }

        private Customer FinCustomerByID(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Customer customer;
            try
            {
                customer = db.Customers.Find(customerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FinCustomerByID", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FinCustomerByID(customerID={0})", customerID);
                throw;
            }

            return customer;
        }

        private async Task<Customer> FinCustomerByIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Customer customer;
            try
            {
                customer = await db.Customers.FindAsync(customerID);

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FinCustomerByIDAsync", timespan.Elapsed, "customerID={0}", customerID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FinCustomerByIDAsync(customerID={0})", customerID);
                throw;
            }

            return customer;
        }

        public SaleModalityCreditRating FindSaleModalityCreditRatingByID(int saleModalityID, int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Customer customer = this.FinCustomerByID(customerID);
                var creditRatingID = customer.CreditRatingID;

                var result =  db.SaleModalityCreditRatings
                    .Where(t => t.CreditRatingID == creditRatingID && t.SaleModalityID == saleModalityID).FirstOrDefault();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingByID", timespan.Elapsed, "saleModalityID={0}, customerID={1}", saleModalityID, customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingByID(saleModalityID={0}, customerID={1})", saleModalityID, customerID);
                throw;
            }
        }

        public async Task<List<SaleModalityCreditRating>> FindSaleModalityCreditRatingBySaleModalityAndCustomerAsync(int saleModalityID, int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Customer customer = await this.FinCustomerByIDAsync(customerID);
                var creditRatingID = customer.CreditRatingID;

                var result = await db.SaleModalityCreditRatings
                    .Where(t => t.CreditRatingID == creditRatingID).ToListAsync();

                if (result != null)
                {
                    foreach (SaleModalityCreditRating item in result)
                    {
                        db.Entry(item).Reference(x => x.SaleModality).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingBySaleModalityAndCustomerAsync", timespan.Elapsed, "saleModalityID={0}, customerID={1}", saleModalityID, customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingBySaleModalityAndCustomerAsync(saleModalityID={0}, customerID={1})", saleModalityID, customerID);
                throw;
            }
        }

        public async Task<List<SaleModalityCreditRating>> FindSaleModalityCreditRatingsBySaleModalityAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.SaleModalityCreditRatings
                    .Where(t => t.SaleModalityID == saleModalityID)
                    .OrderByDescending(t => t.SaleModalityID).ToListAsync();

                if (result != null)
                {
                    foreach (SaleModalityCreditRating item in result)
                    {
                        db.Entry(item).Reference(x => x.CreditRating).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingsBySaleModalityAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FindSaleModalityCreditRatingsBySaleModalityAsync(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public List<CreditRating> FindCreditRatingAvailableBySaleModality(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<SaleModalityCreditRating> saleModalityCreditRatings = db.SaleModalityCreditRatings.Where(p => p.SaleModalityID == saleModalityID).ToList();
                List<CreditRating> creditRatings = db.CreditRatings.ToList();

                foreach (SaleModalityCreditRating item in saleModalityCreditRatings)
                {
                    if (creditRatings.Contains(item.CreditRating))
                    {
                        creditRatings.Remove(item.CreditRating);
                    }
                }


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.FindCreditRatingAvailableByGeographicArea", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return creditRatings;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.FindCreditRatingAvailableByGeographicArea(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public void Create(SaleModalityCreditRating saleModalityCreditRatingToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityCreditRatings.Add(saleModalityCreditRatingToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.Create", timespan.Elapsed, "saleModalityCreditRatingToAdd={0}", saleModalityCreditRatingToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.Create(saleModalityCreditRatingToAdd={0})", saleModalityCreditRatingToAdd);
                throw;
            }
        }

        public Task CreateAsync(SaleModalityCreditRating saleModalityCreditRatingToAdd)
        {
            throw new NotImplementedException();
        }

        public void Delete(int saleModalityCreditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModalityCreditRating smcr = db.SaleModalityCreditRatings.Find(saleModalityCreditRatingID);
                db.SaleModalityCreditRatings.Remove(smcr);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.Delete", timespan.Elapsed, "saleModalityCreditRatingID={0}", saleModalityCreditRatingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.Delete(saleModalityCreditRatingID={0})", saleModalityCreditRatingID);
                throw;
            }
        }

        public async Task DeleteAsync(int saleModalityCreditRatingID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModalityCreditRating smcr = await db.SaleModalityCreditRatings.FindAsync(saleModalityCreditRatingID);
                db.SaleModalityCreditRatings.Remove(smcr);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.DeleteAsync", timespan.Elapsed, "saleModalityCreditRatingID={0}", saleModalityCreditRatingID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.DeleteAsync(saleModalityCreditRatingID={0})", saleModalityCreditRatingID);
                throw;
            }
        }

        public void Update(SaleModalityCreditRating saleModalityCreditRatingToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(saleModalityCreditRatingToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityCreditRatingRepository.Update", timespan.Elapsed, "saleModalityCreditRatingToSave={0}", saleModalityCreditRatingToSave);
            }
            catch (DbEntityValidationException e1)
            {
                log.Error(e1, "Error in SaleModalityCreditRatingRepository.Update(saleModalityCreditRatingToSave={0})", saleModalityCreditRatingToSave);
                throw;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityCreditRatingRepository.Update(saleModalityCreditRatingToSave={0})", saleModalityCreditRatingToSave);
                throw;
            }
        }

        public Task UpdateAsync(SaleModalityCreditRating saleModalityCreditRatingToSave)
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
