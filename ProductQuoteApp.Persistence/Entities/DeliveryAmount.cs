using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(DeliveryAmountMetaData))]
    public class DeliveryAmount
    {
        public int DeliveryAmountID { get; set; }
        public string Description { get; set; }
    }
}
