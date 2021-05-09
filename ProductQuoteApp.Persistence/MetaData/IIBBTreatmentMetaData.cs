using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class IIBBTreatmentMetaData
    {
        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description { get; set; }

        [Display(Name = "PtyPercentage", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        public decimal? Percentage { get; set; }

    }
}
