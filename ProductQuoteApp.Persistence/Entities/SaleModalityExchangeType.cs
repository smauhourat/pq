using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityExchangeType
    {
        public int SaleModalityExchangeTypeID { get; set; }
        public int SaleModalityID { get; set; }
        public int ExchangeTypeID { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual ExchangeType ExchangeType { get; set; }
    }
}
