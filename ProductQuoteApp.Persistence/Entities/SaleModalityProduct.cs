using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityProduct
    {
        public int SaleModalityProductID { get; set; }
        public int SaleModalityID { get; set; }
        public int ProductID { get; set; }

        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        public decimal ProductCost { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual Product Product { get; set; }
    }
}
