using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(SaleModalityCreditRatingMetaData))]
    public class SaleModalityCreditRating
    {
        public int SaleModalityCreditRatingID { get; set; }
        public int SaleModalityID { get; set; }
        public int CreditRatingID { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MinimumMarginPercentage { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MinimumMarginUSD { get; set; }

        public virtual SaleModality SaleModality { get; set; }
        public virtual CreditRating CreditRating { get; set; }

    }
}
