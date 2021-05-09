using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;
        private IProductDocumentRepository productDocumentRepository = null;

        public ProductRepository(ILogger logger, IProductDocumentRepository productDocumentRepo)
        {
            log = logger;
            productDocumentRepository = productDocumentRepo;
        }

        public async Task<Product> CreateCopyAsync(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Product product = await db.Products.FindAsync(productID);
            var newProduct = (Product)db.Entry(product).CurrentValues.ToObject();

            newProduct.Name += " (Copia)";

            try
            {
                db.Products.Add(newProduct);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.CreateCopyAsync", timespan.Elapsed, "newProduct={0}", newProduct);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.CreateCopyAsync(newProduct={0})", newProduct);
                throw;
            }

            return newProduct;
        }

        public async Task CreateAsync(Product productToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Products.Add(productToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.CreateAsync", timespan.Elapsed, "productToAdd={0}", productToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.CreateAsync(productToAdd={0})", productToAdd);
                throw;
            }
        }

        public async Task DeleteAsync(int productID)
        {
            Product product = null;
            Stopwatch timespan = Stopwatch.StartNew();

            ////SiccoAppConfiguration.SuspendExecutionStrategy = true;

            //DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                //productDocumentRepository.DeleteByProductIDAsync(productID);
                 
                product = await db.Products.FindAsync(productID);
                db.Products.Remove(product);
                await db.SaveChangesAsync();

                //tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.DeleteAsync", timespan.Elapsed, "productID={0}", productID);
            }
            catch (Exception e)
            {
                //tran.Rollback();
                log.Error(e, "Error in ProductRepository.DeleteAsync(productID={0})", productID);
                throw;
            }

            ////ProductQuoteApp.SuspendExecutionStrategy = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
                if (productDocumentRepository != null)
                {
                    productDocumentRepository.Dispose();
                    productDocumentRepository = null;
                }
            }
        }

        public async Task<IEnumerable<Product>> FindProductsAsync()
        {
            var result = await db.Products.Include(p => p.Packaging).Include(p => p.Provider).OrderBy(p => p.Name).ToListAsync();
            return result;
        }

        public IEnumerable<Product> FindProducts()
        {
            var result =  db.Products.OrderBy(p => p.Name).ToList();
            return result;
        }

        public Product FindProductsByID(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Product product;
            try
            {
                product = db.Products.Find(productID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.FindProductsByID", timespan.Elapsed, "productID={0}", productID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.FindProductsByID(productID={0})", productID);
                throw;
            }

            return product;
        }

        public async Task<Product> FindProductsByIDAsync(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            Product product;
            try
            {
                //ojooo
                product = await db.Products.FindAsync(productID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.FindProductsByIDAsync", timespan.Elapsed, "productID={0}", productID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.FindProductsByIDAsync(productID={0})", productID);
                throw;
            }

            return product;
        }

        public async Task UpdateAsync(Product productToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(productToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.UpdateAsync", timespan.Elapsed, "productToSave={0}", productToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.UpdateAsync(productToSave={0})", productToSave);
                throw;
            }
        }

        public List<Product> Products()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.Products.OrderBy(p => p.Name).ToList();
                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.Products()", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.Products()");
                throw;
            }
        }

        public List<Product> ProductsActive()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {   
                var result = db.Products
                .Where(t => t.ValidityOfPrice >= DateTime.Now)
                .OrderBy(t => t.Name).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.ProductsActive()", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.ProductsActive()");
                throw;
            }
        }

        public List<Product> ProductsActive(int customerID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {   
                var result = db.Products
                .Where(t => t.ValidityOfPrice >= DateTime.Now)
                .Where(p => p.CustomerProducts.Any(q => q.CustomerID == customerID))
                .OrderBy(t => t.Name).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.ProductsActive", timespan.Elapsed, "(customerID={0}", customerID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.ProductsActive(customerID={0})", customerID);
                throw;
            }
        }
    }
}
