using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(CreditRatingMetaData))]
    public class CreditRating
    {
        public int CreditRatingID { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SaleModalityCreditRating> SaleModalityCreditRatings { get; set; }
        public virtual ICollection<CreditRatingPaymentDeadline> CreditRatingPaymentDeadlines { get; set; }
    }
}
