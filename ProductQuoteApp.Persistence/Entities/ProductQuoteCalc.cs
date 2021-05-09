using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    //[MetadataType(typeof(ProductQuoteMetaData))]
    public class ProductQuoteCalc
    {
        public int ProductQuoteCalcID { get; set; }
        public int ProductQuoteID { get; set; }
        public virtual ProductQuote ProductQuote { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int SaleModalityID { get; set; }
        public virtual SaleModality SaleModality { get; set; }
        public int MinimumQuantityDelivery { get; set; }
        public int QuantityOpenPurchaseOrder { get; set; }
        public int MaximumMonthsStock { get; set; }
        public int GeographicAreaID { get; set; }
        public virtual GeographicArea GeographicArea { get; set; }
        public int PaymentDeadlineID { get; set; }
        public virtual PaymentDeadline PaymentDeadline { get; set; }
        public decimal Price { get; set; }
        public DateTime DateQuote { get; set; }
        public DateTime DateDelivery { get; set; }
        public string ProductQuotePDF { get; set; }
    }
}
