using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityGeographicArea
    {
        public int SaleModalityGeographicAreaID { get; set; }
        public int SaleModalityID { get; set; }
        public int GeographicAreaID { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual GeographicArea GeographicArea { get; set; }
    }
}
