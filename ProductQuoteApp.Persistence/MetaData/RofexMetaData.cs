using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class RofexMetaData
    {
        [Display(Name = "PtyDays", ResourceType = typeof(Resources.Resources))]
        public int Days { get; set; }


        [Display(Name = "PtyDollarQuotation", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal DollarQuotation { get; set; }
    }
}
