using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProductQuoteApp.Persistence
{
    public class ContactTypeRepository : IContactTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public ContactTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public List<ContactType> ContactTypes()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.ContactTypes.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "ContactTypeRepository.ContactTypes", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ContactTypeRepository.ContactTypes()");
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
