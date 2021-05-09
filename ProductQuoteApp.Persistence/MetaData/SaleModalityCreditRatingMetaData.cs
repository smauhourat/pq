using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class SaleModalityCreditRatingMetaData
    {
        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public int SaleModalityID { get; set; }
        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public virtual SaleModality SaleModality { get; set; }


        [Display(Name = "EtyCreditRating", ResourceType = typeof(Resources.Resources))]
        public int CreditRatingID { get; set; }
        [Display(Name = "EtyCreditRating", ResourceType = typeof(Resources.Resources))]
        public virtual CreditRating CreditRating { get; set; }


        [Display(Name = "PtyMinimumMarginPercentage", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyMinimumMarginPercentage_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MinimumMarginPercentage { get; set; }


        [Display(Name = "PtyMinimumMarginUSD", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyMinimumMarginUSD_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MinimumMarginUSD { get; set; }
    }
}
