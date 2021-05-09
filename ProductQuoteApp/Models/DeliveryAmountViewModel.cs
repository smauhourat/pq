using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    [MetadataType(typeof(DeliveryAmountMetaData))]
    public class DeliveryAmountViewModel
    {
        public int DeliveryAmountID { get; set; }
        public string Description { get; set; }

        public DeliveryAmountViewModel(DeliveryAmount deliveryAmount)
        {
            this.DeliveryAmountID = deliveryAmount.DeliveryAmountID;
            this.Description = deliveryAmount.Description;
        }
    }
}