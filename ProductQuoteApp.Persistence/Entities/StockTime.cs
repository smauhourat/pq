using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(StockTimeMetaData))]
    public class StockTime
    {
        public int StockTimeID { get; set; }
        public string Description { get; set; }
        public int Months { get; set; }
    }
}
