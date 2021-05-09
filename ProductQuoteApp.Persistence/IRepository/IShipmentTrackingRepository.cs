using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IShipmentTrackingRepository : IDisposable
    {
        ShipmentTracking FindShipmentTrackingByProductQuoteID(int productQuoteID);

        void Create(ShipmentTracking shipmentTracking);
        Task CreateAsync(ShipmentTracking shipmentTracking);

        void Update(ShipmentTracking shipmentTracking);
        Task UpdateAsync(ShipmentTracking shipmentTracking);

    }
}
