using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class ContactMetaData
    {

        [Display(Name = "EtyContactType", ResourceType = typeof(Resources.Resources))]
        public int ContactTypeID { get; set; }

        [Display(Name = "EtyContactType", ResourceType = typeof(Resources.Resources))]
        public virtual ContactType ContactType { get; set; }

        //[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Contacto")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyDateContact_Required")]
        public DateTime DateContact { get; set; }

        [Display(Name = "Details", ResourceType = typeof(Resources.Resources))]
        [StringLength(2048, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Details2048_Long")]
        public string Details { get; set; }

    }
}
