using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductQuoteApp.Persistence
{
    public class SellerCompanyRepository : ISellerCompanyRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public SellerCompanyRepository(ILogger logger)
        {
            log = logger;
        }

        public List<SellerCompany> SellerCompanys()
        {
            var result = db.SellerCompanys.OrderBy(s => s.Name).ToList();
            return result;
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
