using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;

namespace ProductQuoteApp.Models
{
    public class CustomerFlatViewModel
    {
        public int CustomerID { get; set; }
        public string Company { get; set; }
        public int CreditRatingID { get; set; }
        public Boolean IsSpot { get; set; }
        public int DelayAverageDays { get; set; }
        public string ContactName { get; set; }
        public string ContactTE { get; set; }
        public string ContactEmail { get; set; }

        public CustomerFlatViewModel(Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            this.Company = customer.Company;
            this.CreditRatingID = customer.CreditRatingID;
            this.IsSpot = customer.IsSpot;
            this.DelayAverageDays = customer.DelayAverageDays;
            this.ContactName = customer.ContactName;
            this.ContactTE = customer.ContactTE;
            this.ContactEmail = customer.ContactEmail;
        }

    }
}