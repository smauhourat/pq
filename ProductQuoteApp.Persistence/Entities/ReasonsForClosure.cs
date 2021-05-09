using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ReasonsForClosureMetaData))]
    public class ReasonsForClosure
    {
        public int ReasonsForClosureID { get; set; }
        public string Description { get; set; }

    }
}
