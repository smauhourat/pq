using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityDeliveryAmount
    {
        public int SaleModalityDeliveryAmountID { get; set; }
        public int SaleModalityID { get; set; }
        public int DeliveryAmountID { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual DeliveryAmount DeliveryAmount { get; set; }
    }
}
