using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityGeographicAreaRepository : IDisposable
    {
        Task<List<GeographicArea>> FindGeographicAreasBySaleModalityAsync(int saleModalityID);
        List<GeographicArea> FindGeographicAreasBySaleModality(int saleModalityID);

    }
}
