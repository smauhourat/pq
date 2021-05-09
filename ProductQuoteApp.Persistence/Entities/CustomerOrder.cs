using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(CustomerOrderMetaData))]
    public class CustomerOrder
    {
        
        [Key, ForeignKey("ProductQuote")]
        public int ProductQuoteID { get; set; }
        public virtual ProductQuote ProductQuote { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CustomerOrderCode { get; set; }

        [NotMapped]
        //public string CustomerOrderCodeNew { get { return this.ProductQuote.ProductQuoteCode.Replace("COT", "OC"); }}
        public string CustomerOrderCodeNew { get { return this.ProductQuote != null ? this.ProductQuote.ProductQuoteCode.Replace("COT", "OC") : ""; } }

        public DateTime DateOrder { get; set; }
        public DateTime DateOrderView { get { return this.DateOrder; } }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
            
        public int CustomerOrderStatusID { get; set; }
        public virtual CustomerOrderStatus CustomerOrderStatus { get; set; }

    }
}
