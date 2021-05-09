using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ContactMetaData))]
    public class Contact : ICloneable
    {
        public int ContactID { get; set; }

        public int ContactTypeID { get; set; }
        public virtual ContactType ContactType { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public DateTime DateContact { get; set; }
        public string Details { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
