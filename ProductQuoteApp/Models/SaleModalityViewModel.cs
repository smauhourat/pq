using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class SaleModalityViewModel
    {
        public int SaleModalityID { get; set; }
        public string Description { get; set; }
        public string Resume { get; set; }

        public SaleModalityViewModel(SaleModality saleModality)
        {
            this.SaleModalityID = saleModality.SaleModalityID;
            this.Description = saleModality.Description;
            this.Resume = saleModality.Resume;
        }
    }
}