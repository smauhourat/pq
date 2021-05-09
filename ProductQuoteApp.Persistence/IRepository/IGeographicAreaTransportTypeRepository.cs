using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IGeographicAreaTransportTypeRepository: IDisposable
    {
        List<GeographicAreaTransportType> FindGeographicAreaTransportTypesByGeographicArea(int geographicAreaID);
        Task<List<GeographicAreaTransportType>> FindGeographicAreaTransportTypesByGeographicAreaAsync(int geographicAreaID);
        List<TransportType> FindTransportTypeByGeographicArea(int geographicAreaID);
        List<TransportType> FindTransportTypeAvailableByGeographicArea(int geographicAreaID);
        void Create(GeographicAreaTransportType geographicAreaTransportTypeToAdd);
        Task CreateAsync(GeographicAreaTransportType geographicAreaTransportTypeToAdd);
        void Delete(int geographicAreaTransportTypeID);
        Task DeleteAsync(int geographicAreaTransportTypeID);
        void Update(GeographicAreaTransportType geographicAreaTransportTypeToSave);
        Task UpdateAsync(GeographicAreaTransportType geographicAreaTransportTypeToSave);

    }
}
