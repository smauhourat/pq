using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityDeliveryAmountRepository : IDisposable
    {
        Task<List<DeliveryAmount>> FindDeliveryAmountsBySaleModalityAsync(int saleModalityID);
        List<DeliveryAmount> FindDeliveryAmountsBySaleModality(int saleModalityID);

    }
}
