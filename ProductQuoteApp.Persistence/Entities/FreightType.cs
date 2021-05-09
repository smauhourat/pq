using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(FreightTypeMetaData))]
    public class FreightType
    {
        public int FreightTypeID { get; set; }
        public string Description { get; set; }
    }
}
