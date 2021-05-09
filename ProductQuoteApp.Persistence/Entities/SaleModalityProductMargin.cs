using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityProductMargin
    {
        public int SaleModalityProductMarginID { get; set; }
        public int SaleModalityID { get; set; }
        public int ProductID { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Precision(25, 10)]
        public decimal? MinimumMarginPercentage { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.000}")]
        [Precision(25, 10)]
        public decimal? MinimumMarginUSD { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual Product Product { get; set; }
    }
}
