using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IDeliveryAmountRepository : IDisposable
    {
        List<DeliveryAmount> DeliveryAmounts();
        Task<List<DeliveryAmount>> FindDeliveryAmountsAsync();
        DeliveryAmount FindDeliveryAmountsByID(int deliveryAmountID);
        Task<DeliveryAmount> FindDeliveryAmountsByIDAsync(int deliveryAmountID);
        List<DeliveryAmount> FindDeliveryAmountsBySaleModalityID(int saleModalityID);

    }
}
