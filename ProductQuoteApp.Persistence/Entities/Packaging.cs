using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(PackagingMetaData))]
    public class Packaging
    {
        public int PackagingID { get; set; }
        public string Description { get; set; }
        public Boolean Stackable { get; set; }
    }
}
