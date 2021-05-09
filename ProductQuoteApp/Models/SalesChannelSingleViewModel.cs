using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class SalesChannelSingleViewModel
    {
        public int SalesChannelID { get; set; }
        public string FullName { get; set; }

        public SalesChannelSingleViewModel(SalesChannel saleChannel)
        {
            if (saleChannel == null) return;

            this.SalesChannelID = saleChannel.SalesChannelID;
            this.FullName = saleChannel.Code + " - " + saleChannel.Description;
        }

    }
}