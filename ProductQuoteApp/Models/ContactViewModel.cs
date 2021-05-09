using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    [MetadataType(typeof(ContactMetaData))]
    public class ContactViewModel
    {
        public int CustomerID { get; set; }
        public int ContactID { get; set; }
        public int ContactTypeID { get; set; }
        public ContactType ContactType { get; set; }
        public DateTime DateContact { get; set; }
        public string Details { get; set; }

        public ContactViewModel() { }

        public ContactViewModel(Contact contact)
        {
            this.CustomerID = contact.CustomerID;
            this.ContactID = contact.ContactID;
            this.ContactTypeID = contact.ContactTypeID;
            this.ContactType = contact.ContactType;
            this.DateContact = contact.DateContact;
            this.Details = contact.Details;
        }

        internal Contact GetContact()
        {
            return new Contact()
            {
                ContactID = this.ContactID,
                CustomerID = this.CustomerID,
                ContactTypeID = this.ContactTypeID,
                DateContact = this.DateContact,
                Details = this.Details
            };
        }
    }
}