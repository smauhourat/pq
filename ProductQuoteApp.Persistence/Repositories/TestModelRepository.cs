using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProductQuoteApp.Persistence
{
    public class TestModelRepository: ITestModelRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;


        public TestModelRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(TestModel testModel)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.TestModels.Add(testModel);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "TestModelRepository.Create", timespan.Elapsed, "testModel={0}", testModel);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in TestModelRepository.Create(testModel={0})", testModel);
                throw;
            }
        }

        public List<TestModel> ListAll()
        {
            var result = db.TestModels.ToList();
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
