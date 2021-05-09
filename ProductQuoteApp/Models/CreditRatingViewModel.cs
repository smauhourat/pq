using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class CreditRatingViewModel
    {
        public int CreditRatingID { get; set; }
        public string Description { get; set; }

        public CreditRatingViewModel(CreditRating creditRating)
        {
            this.CreditRatingID = creditRating.CreditRatingID;
            this.Description = creditRating.Description;
        }

        public static CreditRatingViewModel FromCreditRating(CreditRating creditRating)
        {
            return new CreditRatingViewModel(creditRating);
        }
    }
}