using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    class ShipmentTrackingMetaData
    {
        [Display(Name = "PtyStateName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyStateName_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyStateName_Long")]
        public string StateName { get; set; }

        [Display(Name = "PtyEstimatedDate", ResourceType = typeof(Resources.Resources))]
        public DateTime EstimatedDate { get; set; }

        [Display(Name = "PtyRealDate", ResourceType = typeof(Resources.Resources))]
        public DateTime RealDate { get; set; }

        [Display(Name = "PtyCompleted", ResourceType = typeof(Resources.Resources))]
        public Boolean Completed { get; set; }
    }
}
