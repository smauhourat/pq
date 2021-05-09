using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IFreightTypeRepository : IDisposable
    {
        List<FreightType> FreightTypes();
        Task<List<FreightType>> FindFreightTypesAsync();
        FreightType FindFreightTypesByID(int freightTypeID);
        Task<FreightType> FindFreightTypesByIDAsync(int freightTypeID);
        Task CreateAsync(FreightType freightTypeToAdd);
        Task DeleteAsync(int freightTypeID);
        Task UpdateAsync(FreightType freightTypeToSave);
    }
}
