using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ContactTypeMetaData))]
    public class ContactType
    {
        public int ContactTypeID { get; set; }
        public string Description { get; set; }
    }
}
