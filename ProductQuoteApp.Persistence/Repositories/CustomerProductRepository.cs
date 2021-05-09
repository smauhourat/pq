using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CustomerProductRepository : ICustomerProductRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public CustomerProductRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(CustomerProduct customerProductToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CustomerProducts.Add(customerProductToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.Create", timespan.Elapsed, "customerProductToAdd={0}", customerProductToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.Create(customerProductToAdd={0})", customerProductToAdd);
                throw;
            }
        }

        public async Task CreateAsync(CustomerProduct customerProductToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.CustomerProducts.Add(customerProductToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.CreateAsync", timespan.Elapsed, "customerProductToAdd={0}", customerProductToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.CreateAsync(customerProductToAdd={0})", customerProductToAdd);
                throw;
            }
        }

        public void Delete(int customerProductID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CustomerProduct customerProduct = db.CustomerProducts.Find(customerProductID);
                db.CustomerProducts.Remove(customerProduct);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.Delete", timespan.Elapsed, "customerProductID={0}", customerProductID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.Delete(customerProductID={0})", customerProductID);
                throw;
            }
        }

        public async Task DeleteAsync(int customerProductID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CustomerProduct customerProduct = await db.CustomerProducts.FindAsync(customerProductID);
                db.CustomerProducts.Remove(customerProduct);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.DeleteAsync", timespan.Elapsed, "customerProductID={0}", customerProductID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.DeleteAsync(customerProductID={0})", customerProductID);
                throw;
            }
        }

        public void DeleteByCustomer(int customerID)
        {
            var customerProducts = this.FindCustomerProductsByCustomerID(customerID);
            if (customerProducts != null)
            {
                foreach (CustomerProduct cp in customerProducts)
                {
                    this.Delete(cp.CustomerProductID);
                }
            }
        }

        public void AddAllProductsToCustomer(int customerID)
        {
            AddAllProductsToCustomer(customerID, false);
        }

        public void AddAllProductsToCustomer(int customerID, bool calculationDetails)
        {
            var products = this.FindProductsAvailableByCustomer(customerID);

            foreach (Product p in products)
            {
                CustomerProduct cp = new CustomerProduct
                {
                    CustomerID = customerID,
                    ProductID = p.ProductID,
                    CalculationDetails = calculationDetails
                };
                this.Create(cp);
            }
        }

        public void AddAllCustomersToProduct(int productID)
        {
            var customers = this.FindCustomerNoAssignedToProduct(productID);

            foreach (Customer c in customers)
            {
                CustomerProduct cp = new CustomerProduct
                {
                    CustomerID = c.CustomerID,
                    ProductID = productID,
                    CalculationDetails = c.IsSpot //si es Spot visualiza el Costeo
                };
                this.Create(cp);
            }
        }

        public void AddAllSpotCustomerToProduct(int productID)
        {
            var customers = this.FindCustomerNoAssignedToProduct(productID);

            foreach (Customer c in customers)
            {
                if (c.IsSpot)
                {
                    CustomerProduct cp = new CustomerProduct
                    {
                        CustomerID = c.CustomerID,
                        ProductID = productID,
                        CalculationDetails = true //si es Spot visualiza el Costeo
                    };
                    this.Create(cp);
                }
            }
        }

        public List<CustomerProduct> FindCustomerProductsByCustomerID(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.CustomerProducts
                    .Where(t => t.CustomerID == customerID)
                    .OrderByDescending(t => t.Product.Name).ToList();

                if (result != null)
                {
                    foreach (CustomerProduct item in result)
                    {
                        db.Entry(item).Reference(x => x.Product).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.FindCustomerProductsByCustomerID", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.FindCustomerProductsByCustomerID(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<List<CustomerProduct>> FindCustomerProductsByCustomerIDAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.CustomerProducts
                    .Include(p => p.Product.Packaging)
                    .Where(t => t.CustomerID == customerID)
                    .OrderBy(t => t.Product.Name).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.FindCustomerProductsByCustomerIDAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.FindCustomerProductsByCustomerIDAsync(customerID={0})", customerID);
                throw;
            }
        }

        public List<Product> FindProductsAvailableByCustomer(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<CustomerProduct> customerProducts = db.CustomerProducts.Where(p => p.CustomerID == customerID).ToList();
                List<Product> products = db.Products.ToList();

                foreach (CustomerProduct item in customerProducts)
                {
                    if (products.Contains(item.Product))
                    {
                        products.Remove(item.Product);
                    }
                }


                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.FindProductsAvailableByCustomer", timespan.Elapsed, "customerID={0}", customerID);

                return products;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.FindProductsAvailableByCustomer(customerID={0})", customerID);
                throw;
            }
        }


        public List<Customer> FindCustomerNoAssignedToProduct(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<CustomerProduct> customerProducts = db.CustomerProducts.Where(p => p.ProductID == productID).ToList();
                List<Customer> customers = db.Customers.ToList();

                foreach (CustomerProduct item in customerProducts)
                {
                    if (customers.Contains(item.Customer))
                    {
                        customers.Remove(item.Customer);
                    }
                }
                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.FindCustomerNoAssignedToProduct", timespan.Elapsed, "productID={0}", productID);

                return customers;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.FindCustomerNoAssignedToProduct(productID={0})", productID);
                throw;
            }

        }

        public async Task<List<Product>> FindProductsAvailableByCustomerAsync(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<CustomerProduct> customerProducts = await db.CustomerProducts.Where(p => p.CustomerID == customerID).ToListAsync();
                List<Product> products = await db.Products.Include(p => p.Packaging).ToListAsync();

                foreach (CustomerProduct item in customerProducts)
                {
                    if (products.Contains(item.Product))
                    {
                        products.Remove(item.Product);
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.FindProductsAvailableByCustomerAsync", timespan.Elapsed, "customerID={0}", customerID);

                return products;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.FindProductsAvailableByCustomerAsync(customerID={0})", customerID);
                throw;
            }
        }

        public void Update(CustomerProduct customerProductToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(customerProductToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.Update", timespan.Elapsed, "customerProductToSave={0}", customerProductToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.Update(customerProductToSave={0})", customerProductToSave);
                throw;
            }
        }

        public async Task UpdateAsync(CustomerProduct customerProductToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(customerProductToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.Update", timespan.Elapsed, "customerProductToSave={0}", customerProductToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.Update(customerProductToSave={0})", customerProductToSave);
                throw;
            }
        }

        public Boolean CanShowCosteo(int customerID, int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Boolean result = false;

            try
            {
                CustomerProduct customerProduct = db.CustomerProducts
                    .Where(t => t.CustomerID == customerID)
                    .Where(p => p.ProductID == productID).FirstOrDefault();

                if (customerProduct != null)
                {
                    result = customerProduct.CalculationDetails;
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.CanShowCosteo", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.CanShowCosteo(customerID={0})", customerID);
                throw;
            }
        }

        public async Task<Boolean> CanShowCosteoAsync(int customerID, int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Boolean result = false;

            try
            {
                CustomerProduct customerProduct = await db.CustomerProducts
                    .Where(t => t.CustomerID == customerID)
                    .Where(p => p.ProductID == productID).FirstOrDefaultAsync();

                if (customerProduct != null)
                {
                    result = customerProduct.CalculationDetails;
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "CustomerProductRepository.CanShowCosteoAsync", timespan.Elapsed, "customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in CustomerProductRepository.CanShowCosteoAsync(customerID={0})", customerID);
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
