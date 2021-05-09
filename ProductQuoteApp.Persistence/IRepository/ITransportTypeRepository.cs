using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ITransportTypeRepository : IDisposable
    {
        List<TransportType> TransportTypesByGeographicArea(int geographicAreaID);
        List<TransportType> FindTransportTypes();
        Task<List<TransportType>> FindTransportTypesAsync();
        Task<TransportType> FindTransportTypesByIDAsync(int transportTypeID);
        Task CreateAsync(TransportType transportTypeToAdd);
        Task DeleteAsync(int transportTypeID);
        Task UpdateAsync(TransportType transportTypeToSave);
    }
}
