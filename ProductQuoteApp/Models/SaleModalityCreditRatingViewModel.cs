using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class SaleModalityCreditRatingViewModel
    {
        public int SaleModalityCreditRatingID { get; set; }
        public int SaleModalityID { get; set; }
        public string SaleModalityDescription { get; set; }
        public int CreditRatingID { get; set; }
        public string CreditRatingDescription { get; set; }
        public decimal MinimumMarginPercentage { get; set; }
        public decimal MinimumMarginUSD { get; set; }

        public SaleModalityCreditRatingViewModel(SaleModalityCreditRating saleModalityCreditRating)
        {
            this.SaleModalityCreditRatingID = saleModalityCreditRating.SaleModalityCreditRatingID;
            this.SaleModalityID = saleModalityCreditRating.SaleModalityID;
            this.SaleModalityDescription = saleModalityCreditRating.SaleModality.Description;
            this.CreditRatingID = saleModalityCreditRating.CreditRatingID;
            this.CreditRatingDescription = saleModalityCreditRating.CreditRating.Description;
            this.MinimumMarginPercentage = saleModalityCreditRating.MinimumMarginPercentage;
            this.MinimumMarginUSD = saleModalityCreditRating.MinimumMarginUSD;
        }
    }
}