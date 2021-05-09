using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class SalesChannelMetaData
    {

        [Display(Name = "Code_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Code_Required")]
        public string Code { get; set; }

        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description { get; set; }

    }
}
