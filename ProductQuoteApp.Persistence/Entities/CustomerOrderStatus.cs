using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    public class CustomerOrderStatus
    {
        public int CustomerOrderStatusID { get; set; }

        [Display(Name = "PtyOCStatus", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }
    }
}
