using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class SaleModalityProductViewModel
    {
        public int SaleModalityProductID { get; set; }
        public int SaleModalityID { get; set; }
        public string SaleModalityDescription { get; set; }
        public int ProductID { get; set; }
        public string ProductFullName { get; set; }
        public decimal ProductCost { get; set; }

        public SaleModalityProductViewModel(SaleModalityProduct saleModalityProduct)
        {
            this.SaleModalityProductID = saleModalityProduct.SaleModalityProductID;
            this.SaleModalityID = saleModalityProduct.SaleModalityID;
            this.SaleModalityDescription = saleModalityProduct.SaleModality.Description;
            this.ProductID = saleModalityProduct.ProductID;
            this.ProductFullName = saleModalityProduct.Product.FullName;
            this.ProductCost = saleModalityProduct.ProductCost;
        }

    }
}