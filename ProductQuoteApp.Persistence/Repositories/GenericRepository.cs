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
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        private ApplicationDbContext db = null; 
        private DbSet<T> table = null;
        private readonly ILogger log = null;

        public GenericRepository(ILogger logger)
        {
            this.db = new ApplicationDbContext();
            this.table = db.Set<T>();
            log = logger;
        }

        public List<T> GetAll()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = this.table.ToList();

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.GetAll", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.GetAll()");
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await this.table.ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.GetAll", timespan.Elapsed);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.GetAll()");
                throw;
            }
        }

        public async Task<T> GetByIDAsync(int id)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            T entity;
            try
            {
                entity = await table.FindAsync(id);

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.GetByIDAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.GetByIDAsync(id={0})", id);
                throw;
            }

            return entity;
        }

        public async Task CreateAsync(T entity)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                table.Add(entity);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.CreateAsync", timespan.Elapsed, "entity={0}", entity);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.CreateAsync(entity={0})", entity);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                T entity = await table.FindAsync(id);
                table.Remove(entity);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.DeleteAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.DeleteAsync(id={0})", id);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GenericRepository.UpdateAsync", timespan.Elapsed, "entity={0}", entity);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GenericRepository.UpdateAsync(entity={0})", entity);
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
            if (disposing)
            {
                // Free managed resources
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }

                table = null;
            }
        }
    }
}
