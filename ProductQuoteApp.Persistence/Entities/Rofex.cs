using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(RofexMetaData))]
    public class Rofex
    {
        public int RofexID { get; set; }
        public int Days { get; set; }
        public decimal DollarQuotation { get; set; }
    }
}
