using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class CustomerProductViewModel
    {
        public int CustomerID { get; set; }

        public ICollection<CustomerProduct> AssignedProducts { get; set; }
        public ICollection<Product> AvailableProducts { get; set; }


    }
}