using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class ProductDocumentMetaData
    {
        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description { get; set; }

        [Display(Name = "PtyProductDocumentPDF", ResourceType = typeof(Resources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyProductDocumentPDF_Required")]
        public string ProductDocumentPDF { get; set; }
    }
}
