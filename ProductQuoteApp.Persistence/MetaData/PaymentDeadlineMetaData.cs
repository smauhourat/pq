using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class PaymentDeadlineMetaData
    {
        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description{ get; set; }

        [Display(Name = "PtyDays", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyDays_Required")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Days { get; set; }

        [Display(Name = "PtyMonths", ResourceType = typeof(Resources.Resources))]
        public decimal Months { get; set; }
    }
}
