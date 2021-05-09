using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class CustomerProductSingleViewModel
    {
        public int CustomerProductID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Boolean CalculationDetails { get; set; }

        public CustomerProductSingleViewModel(CustomerProduct customerProduct)
        {
            if (customerProduct == null) return;

            this.CustomerProductID = customerProduct.CustomerProductID;
            this.CustomerID = customerProduct.CustomerID;
            this.CustomerName = customerProduct.Customer.Company;
            this.ProductID = customerProduct.ProductID;
            this.ProductName = customerProduct.Product.FullName;
            this.CalculationDetails = customerProduct.CalculationDetails;
        }
    }
}