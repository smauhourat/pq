using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ProviderMetaData))]
    public class Provider
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
    }
}
