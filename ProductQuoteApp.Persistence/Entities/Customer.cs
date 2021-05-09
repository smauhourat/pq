using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
//MargenMin
namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(CustomerMetaData))]
    public class Customer : ICloneable
    {
        public int CustomerID { get; set; }
        public string Company { get; set; }
        public int CreditRatingID { get; set; }        
        public virtual CreditRating CreditRating { get; set; }
        public decimal? MinimumMarginPercentage { get; set; }
        public decimal? MinimumMarginUSD { get; set; }
        public decimal SellerCommission { get; set; }
        public virtual ICollection<CustomerUser> Users { get; set; }
        public string Observation { get; set; }
        public Boolean IsSpot { get; set; }
        public Boolean IsProspect { get; set; }

        public Boolean SendNotifications { get; set; }

        public int DelayAverageDays { get; set; }


        public string ContactName { get; set; }
        public string ContactTE { get; set; }
        public string ContactEmail { get; set; }
        public int? ContactClassID { get; set; }
        public ContactClass ContactClass { get; set; }

        public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }

        public virtual ICollection<SaleModalityCustomerMargin> SaleModalityCustomerMargins { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
