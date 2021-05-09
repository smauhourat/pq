using System.ComponentModel.DataAnnotations;


namespace ProductQuoteApp.Persistence
{
    public class ProviderMetaData
    {

        [Display(Name = "Name_Name", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name200_Long")]
        public string ProviderName { get; set; }
    }
}
