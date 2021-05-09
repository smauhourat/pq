using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityProductRepository : ISaleModalityProductRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SaleModalityProductRepository(ILogger logger)
        {
            log = logger;
        }

        public List<SaleModalityProduct> FindProductsBySaleModality(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.SaleModalityProducts
                    .Where(t => t.SaleModalityID == saleModalityID)
                    .OrderByDescending(t => t.Product.Name).ToList();

                if (result != null)
                {
                    foreach (SaleModalityProduct item in result)
                    {
                        db.Entry(item).Reference(x => x.Product).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindProductsBySaleModality", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindProductsBySaleModality(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public async Task<List<SaleModalityProduct>> FindProductsBySaleModalityAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.SaleModalityProducts
                    .Where(t => t.SaleModalityID == saleModalityID)
                    .OrderByDescending(t => t.Product.Name).ToListAsync();

                if (result != null)
                {
                    foreach (SaleModalityProduct item in result)
                    {
                        db.Entry(item).Reference(x => x.Product).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindProductsBySaleModalityAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindProductsBySaleModalityAsync(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public List<SaleModality> FindSaleModalityByProduct(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                Product product = db.Products.Where(p => p.ProductID == productID).Single();
                var salesModalitys = product.SaleModalityProducts.Select(s => s.SaleModality).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindSaleModalityByProduct", timespan.Elapsed, "productID={0}", productID);

                return salesModalitys;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindSaleModalityByProduct(productID={0})", productID);
                throw;
            }
            
        }

        public SaleModalityProduct FindBySaleModalityAndProduct(int saleModalityID, int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.SaleModalityProducts.Where(p => p.ProductID == productID).Where(s => s.SaleModalityID == saleModalityID).Single();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindBySaleModalityAndProduct", timespan.Elapsed, "saleModalityID={0}, productID={1}", saleModalityID, productID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindBySaleModalityAndProduct(saleModalityID={0}, productID={0})", saleModalityID, productID);
                throw;
            }
        }

        public async Task<List<SaleModalityProduct>> FindSaleModalityProductsBySaleModalityAsync(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.SaleModalityProducts
                    .Where(t => t.SaleModalityID == saleModalityID)
                    .OrderByDescending(t => t.SaleModalityID).ToListAsync();

                if (result != null)
                {
                    foreach (SaleModalityProduct item in result)
                    {
                        db.Entry(item).Reference(x => x.Product).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindSaleModalityProductsBySaleModalityAsync", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindSaleModalityProductsBySaleModalityAsync(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public List<Product> FindProductAvailableBySaleModality(int saleModalityID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<SaleModalityProduct> saleModalityProducts = db.SaleModalityProducts.Where(p => p.SaleModalityID == saleModalityID).ToList();
                List<Product> products = db.Products.ToList();

                foreach (SaleModalityProduct item in saleModalityProducts)
                {
                    if (products.Contains(item.Product))
                    {
                        products.Remove(item.Product);
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.FindProductAvailableBySaleModality", timespan.Elapsed, "saleModalityID={0}", saleModalityID);

                return products;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.FindProductAvailableBySaleModality(saleModalityID={0})", saleModalityID);
                throw;
            }
        }

        public void Create(SaleModalityProduct saleModalityProductToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProducts.Add(saleModalityProductToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.Create", timespan.Elapsed, "saleModalityProductToAdd={0}", saleModalityProductToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.Create(saleModalityProductToAdd={0})", saleModalityProductToAdd);
                throw;
            }
        }

        public async Task CreateAsync(SaleModalityProduct saleModalityProductToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProducts.Add(saleModalityProductToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.CreateAsync", timespan.Elapsed, "productSaleModalityToAdd={0}", saleModalityProductToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.CreateAsync(productSaleModalityToAdd={0})", saleModalityProductToAdd);
                throw;
            }
        }

        public void Update(SaleModalityProduct saleModalityProductToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(saleModalityProductToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.Update", timespan.Elapsed, "saleModalityProductToSave={0}", saleModalityProductToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.Update(saleModalityProductToSave={0})", saleModalityProductToSave);
                throw;
            }
        }

        public void Delete(int saleModalityProductID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                SaleModalityProduct smp = db.SaleModalityProducts.Find(saleModalityProductID);
                db.SaleModalityProducts.Remove(smp);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.Delete", timespan.Elapsed, "saleModalityProductID={0}", saleModalityProductID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.Delete(saleModalityProductID={0})", saleModalityProductID);
                throw;
            }
        }

        public void DeleteByProduct(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.SaleModalityProducts.RemoveRange(db.SaleModalityProducts.Where(p => p.ProductID == productID));
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "SaleModalityProductRepository.DeleteByProduct", timespan.Elapsed, "productID={0}", productID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in SaleModalityProductRepository.DeleteByProduct(productID={0})", productID);
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
