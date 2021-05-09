using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class CustomerOrderViewModel
    {
        public int ProductQuoteID { get; set; }
        public virtual ProductQuoteViewModel ProductQuote { get; set; }

        public string CustomerOrderCode { get; set; }

        public string CustomerOrderCodeNew { get; set; }

        public DateTime DateOrder { get; set; }
        public DateTime DateOrderView { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }

        public int CustomerOrderStatusID { get; set; }
        public virtual CustomerOrderStatus CustomerOrderStatus { get; set; }

        public CustomerOrderViewModel(CustomerOrder co)
        {
            this.ProductQuoteID = co.ProductQuoteID;
            this.CustomerOrderCode = co.CustomerOrderCode;
            this.CustomerOrderCodeNew = co.CustomerOrderCodeNew;
            this.DateOrder = co.DateOrder;
            this.DateOrderView = co.DateOrderView;
            this.ApprovedDate = co.ApprovedDate;
            this.RejectedDate = co.RejectedDate;
            this.CustomerOrderStatus = co.CustomerOrderStatus;

            this.ProductQuote = new Models.ProductQuoteViewModel(co.ProductQuote);
        }

    }
}