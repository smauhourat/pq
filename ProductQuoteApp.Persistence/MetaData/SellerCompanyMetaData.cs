using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class SellerCompanyMetaData
    {
        public int SellerCompanyID { get; set; }

        [Display(Name = "Name_Name", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name200_Long")]
        public string Name { get; set; }

        [Display(Name = "PtyPQPDFTemplate", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPQPDFTemplate_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPQPDFTemplate200_Long")]
        public string ProductQuotePdfTemplate { get; set; }

        [Display(Name = "PtyPQSmallPDFTemplate", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPQSmallPDFTemplate_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPQSmallPDFTemplate200_Long")]
        public string ProductQuoterSmallPdfTemplate { get; set; }
    }
}
