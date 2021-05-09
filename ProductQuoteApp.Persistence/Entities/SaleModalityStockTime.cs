using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityStockTime
    {
        public int SaleModalityStockTimeID { get; set; }
        public int SaleModalityID { get; set; }
        public int StockTimeID { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual StockTime StockTime { get; set; }
    }
}
