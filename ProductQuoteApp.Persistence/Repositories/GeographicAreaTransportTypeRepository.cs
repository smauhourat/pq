using ProductQuoteApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class GeographicAreaTransportTypeRepository : IGeographicAreaTransportTypeRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger log = null;

        public GeographicAreaTransportTypeRepository(ILogger logger)
        {
            log = logger;
        }

        public void Create(GeographicAreaTransportType geographicAreaTransportTypeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.GeographicAreaTransportTypes.Add(geographicAreaTransportTypeToAdd);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.Create", timespan.Elapsed, "geographicAreaTransportTypeToAdd={0}", geographicAreaTransportTypeToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.Create(geographicAreaTransportTypeToAdd={0})", geographicAreaTransportTypeToAdd);
                throw;
            }
        }

        public async Task CreateAsync(GeographicAreaTransportType geographicAreaTransportTypeToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.GeographicAreaTransportTypes.Add(geographicAreaTransportTypeToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.CreateAsync", timespan.Elapsed, "geographicAreaTransportTypeToAdd={0}", geographicAreaTransportTypeToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.CreateAsync(geographicAreaTransportTypeToAdd={0})", geographicAreaTransportTypeToAdd);
                throw;
            }
        }

        public void Delete(int geographicAreaTransportTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                GeographicAreaTransportType gatp = db.GeographicAreaTransportTypes.Find(geographicAreaTransportTypeID);
                db.GeographicAreaTransportTypes.Remove(gatp);
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.Delete", timespan.Elapsed, "geographicAreaTransportTypeID={0}", geographicAreaTransportTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.Delete(geographicAreaTransportTypeID={0})", geographicAreaTransportTypeID);
                throw;
            }
        }
        public async Task DeleteAsync(int geographicAreaTransportTypeID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                GeographicAreaTransportType gatp = await db.GeographicAreaTransportTypes.FindAsync(geographicAreaTransportTypeID);
                db.GeographicAreaTransportTypes.Remove(gatp);
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.DeleteAsync", timespan.Elapsed, "geographicAreaTransportTypeID={0}", geographicAreaTransportTypeID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.DeleteAsync(geographicAreaTransportTypeID={0})", geographicAreaTransportTypeID);
                throw;
            }
        }

        public void Update(GeographicAreaTransportType geographicAreaTransportTypeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(geographicAreaTransportTypeToSave).State = EntityState.Modified;
                db.SaveChanges();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.Update", timespan.Elapsed, "geographicAreaTransportTypeToSave={0}", geographicAreaTransportTypeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.Update(geographicAreaTransportTypeToSave={0})", geographicAreaTransportTypeToSave);
                throw;
            }
        }

        public async Task UpdateAsync(GeographicAreaTransportType geographicAreaTransportTypeToSave)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.Entry(geographicAreaTransportTypeToSave).State = EntityState.Modified;
                await db.SaveChangesAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.UpdateAsync", timespan.Elapsed, "geographicAreaTransportTypeToSave={0}", geographicAreaTransportTypeToSave);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.UpdateAsync(geographicAreaTransportTypeToSave={0})", geographicAreaTransportTypeToSave);
                throw;
            }
        }

        public List<GeographicAreaTransportType> FindGeographicAreaTransportTypesByGeographicArea(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = db.GeographicAreaTransportTypes
                    .Where(t => t.GeographicAreaID == geographicAreaID)
                    .OrderByDescending(t => t.GeographicAreaID).ToList();

                if (result != null)
                {
                    foreach (GeographicAreaTransportType item in result)
                    {
                        db.Entry(item).Reference(x => x.TransportType).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicArea", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicArea(geographicAreaID={0})", geographicAreaID);
                throw;
            }
        }

        public async Task<List<GeographicAreaTransportType>> FindGeographicAreaTransportTypesByGeographicAreaAsync(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.GeographicAreaTransportTypes
                    .Where(t => t.GeographicAreaID == geographicAreaID)
                    .OrderBy(t => t.TransportType.Description).ToListAsync();

                if (result != null)
                {
                    foreach (GeographicAreaTransportType item in result)
                    {
                        db.Entry(item).Reference(x => x.TransportType).Load();
                    }
                }

                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicAreaAsync", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicAreaAsync(geographicAreaID={0})", geographicAreaID);
                throw;
            }
        }

        public List<TransportType> FindTransportTypeByGeographicArea(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                GeographicArea geographicArea = db.GeographicAreas.Where(p => p.GeographicAreaID == geographicAreaID).Single();
                var result = geographicArea.GeographicAreaTransportTypes.Select(s => s.TransportType).ToList();


                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.FindTransportTypeByGeographicArea", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.FindTransportTypeByGeographicArea(geographicAreaID={0})", geographicAreaID);
                throw;
            }
        }

        public List<TransportType> FindTransportTypeAvailableByGeographicArea(int geographicAreaID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                List<GeographicAreaTransportType> geographicAreaTransportTypes = db.GeographicAreaTransportTypes.Where(p => p.GeographicAreaID == geographicAreaID).ToList();
                List<TransportType> transportTypes = db.TransportTypes.ToList();

                foreach (GeographicAreaTransportType item in geographicAreaTransportTypes)
                {
                    if (transportTypes.Contains(item.TransportType))
                    {
                        transportTypes.Remove(item.TransportType);
                    }
                }


                timespan.Stop();
                log.TraceApi("SQL Database", "GeographicAreaTransportTypeRepository.FindTransportTypeAvailableByGeographicArea", timespan.Elapsed, "geographicAreaID={0}", geographicAreaID);

                return transportTypes;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in GeographicAreaTransportTypeRepository.FindTransportTypeAvailableByGeographicArea(geographicAreaID={0})", geographicAreaID);
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
