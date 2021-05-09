using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ContactClassMetaData))]
    public class ContactClass
    {
        public int ContactClassID { get; set; }
        public string Description { get; set; }
    }
}
