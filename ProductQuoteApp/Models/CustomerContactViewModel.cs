using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class CustomerContactViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public CustomerContactViewModel(Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            this.CustomerName = customer.Company;
            this.Contacts = customer.Contacts;
        }
    }
}