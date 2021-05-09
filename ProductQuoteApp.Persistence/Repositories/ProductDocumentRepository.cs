using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ProductDocumentRepository : IProductDocumentRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ProductDocumentRepository(ILogger logger)
        {
            log = logger;
        }

        public async Task CreateAsync(ProductDocument productDocumentToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.ProductDocuments.Add(productDocumentToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductDocumentRepository.CreateAsync", timespan.Elapsed, "productDocumentToAdd={0}", productDocumentToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentRepository.CreateAsync(productDocumentToAdd={0})", productDocumentToAdd);
                throw;
            }
        }

        public void Delete(int productDocumentID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                ProductDocument productDocument = db.ProductDocuments.Find(productDocumentID);
                db.ProductDocuments.Remove(productDocument);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductDocumentRepository.Delete", timespan.Elapsed, "productDocumentID={0}", productDocumentID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentRepository.Delete(productDocumentID={0})", productDocumentID);
                throw;
            }
        }

        public async Task DeleteAsync(int productDocumentID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                ProductDocument productDocument = await db.ProductDocuments.FindAsync(productDocumentID);
                db.ProductDocuments.Remove(productDocument);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductDocumentRepository.DeleteAsync", timespan.Elapsed, "productDocumentID={0}", productDocumentID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentRepository.DeleteAsync(productDocumentID={0})", productDocumentID);
                throw;
            }
        }

        public Task<List<ProductDocument>> FindProductDocumentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDocument> FindProductDocumentsByIDAsync(int productDocumentID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            ProductDocument productDocument;
            try
            {
                productDocument = await db.ProductDocuments.FindAsync(productDocumentID);

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductRepository.FindProductDocumentsByIDAsync", timespan.Elapsed, "productDocumentID={0}", productDocumentID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductRepository.FindProductDocumentsByIDAsync(productDocumentID={0})", productDocumentID);
                throw;
            }

            return productDocument;
        }

        public List<ProductDocument> FindProductDocumentsByProductID(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.ProductDocuments
                    .Where(t => t.ProductID == productID)
                    .OrderByDescending(t => t.ProductID).ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductDocumentRepository.FindProductDocumentsByProductID", timespan.Elapsed, "productID={0}", productID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentRepository.FindProductDocumentsByProductID(productID={0})", productID);
                throw;
            }
        }

        public async Task<List<ProductDocument>> FindProductDocumentsByProductIDAsync(int productID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.ProductDocuments
                    .Where(t => t.ProductID == productID)
                    .OrderByDescending(t => t.ProductID).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "ProductDocumentRepository.FindProductDocumentsByProductIDAsync", timespan.Elapsed, "productID={0}", productID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentRepository.FindProductDocumentsByProductIDAsync(productID={0})", productID);
                throw;
            }
        }

        public Task UpdateAsync(ProductDocument productDocumentToSave)
        {
            throw new NotImplementedException();
        }

        public void DeleteByProductIDAsync(int productID)
        {
            var productDocuments = this.FindProductDocumentsByProductID(productID);
            if (productDocuments != null)
            {
                foreach (ProductDocument pd in productDocuments)
                {
                    this.Delete(pd.ProductDocumentID);
                }
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
