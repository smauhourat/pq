using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CustomerProduct
    {
        public int CustomerProductID { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public Boolean CalculationDetails { get; set; }
    }
}
