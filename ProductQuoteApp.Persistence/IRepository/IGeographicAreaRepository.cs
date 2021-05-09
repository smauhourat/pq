using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IGeographicAreaRepository : IDisposable
    {
        List<GeographicArea> GeographicAreas();
        Task<List<GeographicArea>> FindGeographicAreasAsync();
        Task<GeographicArea> FindGeographicAreasByIDAsync(int geographicAreaID);
        List<GeographicArea> FindGeographicAreasBySaleModalityID(int saleModalityID);
        Task CreateAsync(GeographicArea geographicAreaToAdd);
        Task DeleteAsync(int geographicAreaID);
        Task UpdateAsync(GeographicArea geographicAreaToSave);
    }
}
