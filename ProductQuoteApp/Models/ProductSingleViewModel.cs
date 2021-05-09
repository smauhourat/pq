using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class ProductSingleViewModel
    {
        public int ProductID { get; set; }
        public string FullName { get; set; }

        public ProductSingleViewModel(Product product)
        {
            if (product == null) return;

            this.ProductID = product.ProductID;
            this.FullName = product.FullName;
        }
    }
}