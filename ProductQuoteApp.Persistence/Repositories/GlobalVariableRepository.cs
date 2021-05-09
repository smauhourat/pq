using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class GlobalVariableRepository : IGlobalVariableRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public GlobalVariableRepository(ILogger logger)
        {
            log = logger;
        }

        public GlobalVariable FindGlobalVariables()
        {
            GlobalVariable globalVariable = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                globalVariable = db.GlobalVariables.Where(g => g.GlobalVariableID == 1).Single();

                timespan.Stop();
                log.TraceApi("SQL Database", "GlobalVariableRepository.FindGlobalVariables()", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GlobalVariableRepository.FindGlobalVariables()");
                throw;
            }

            return globalVariable;
        }

        public async Task<GlobalVariable> FindGlobalVariablesAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            GlobalVariable globalVariable;
            try
            {
                globalVariable = await db.GlobalVariables.SingleAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GlobalVariableRepository.FindGlobalVariablesAsync()", timespan.Elapsed);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GlobalVariableRepository.FindGlobalVariablesAsync()");
                throw;
            }

            return globalVariable;
        }

        public async Task UpdateAsync(GlobalVariable globalVariableToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(globalVariableToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GlobalVariableRepository.UpdateAsync", timespan.Elapsed, "globalVariableToSave={0}", globalVariableToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GlobalVariableRepository.UpdateAsync(globalVariableToSave={0})", globalVariableToSave);
                throw;
            }
        }

        public void Update(GlobalVariable globalVariableToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(globalVariableToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "GlobalVariableRepository.Update", timespan.Elapsed, "globalVariableToSave={0}", globalVariableToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GlobalVariableRepository.Update(globalVariableToSave={0})", globalVariableToSave);
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
