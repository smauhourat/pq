using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class PackagingMetaData
    {
        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description{ get; set; }

        [Display(Name = "PtyStackable", ResourceType = typeof(Resources.Resources))]
        public Boolean Stackable { get; set; }

    }
}
