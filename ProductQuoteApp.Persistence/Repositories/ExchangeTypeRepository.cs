using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductQuoteApp.Persistence
{
    public class ExchangeTypeRepository : IExchangeTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ExchangeTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<ExchangeType> ExchangeTypes()
        {
            var result = db.ExchangeTypes.ToList();
            return result;
        }

        public List<ExchangeType> FindExchangeTypesBySaleModalityID(int saleModalityID)
        {
            var result = db.SaleModalityExchangeTypes.Where(s => s.SaleModalityID == saleModalityID).Select(s => s.ExchangeType).ToList();

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
