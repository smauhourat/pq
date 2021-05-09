using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class SalesChannelUserSingleViewModel
    {
        public int SalesChannelUserID { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }

        public int SalesChannelID { get; set; }
        public string SalesChannelName { get; set; }

        public SalesChannelUserSingleViewModel(SalesChannelUser saleChannelUser)
        {
            if (saleChannelUser == null) return;

            this.SalesChannelUserID = saleChannelUser.SalesChannelUserID;

            this.UserID = saleChannelUser.UserID;
            this.UserName = saleChannelUser.UserID.ToString();

            this.SalesChannelID = saleChannelUser.SalesChannelID;
            this.SalesChannelName = saleChannelUser.SalesChannel.Code + " - " + saleChannelUser.SalesChannel.Description;


        }

    }
}