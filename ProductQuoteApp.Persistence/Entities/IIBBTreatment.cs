using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(IIBBTreatmentMetaData))]
    public class IIBBTreatment
    {
        public int IIBBTreatmentID { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
    }
}
