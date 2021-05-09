using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(SalesChannelMetaData))]
    public class SalesChannel
    {
        public int SalesChannelID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
