using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class TransportTypeMetaData
    {
        [Display(Name = "Description_Description", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Description200_Long")]
        public string Description{ get; set; }

        [Display(Name = "PtyPositionQuantityFrom", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPositionQuantityFrom_Required")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIntRange1_1000000")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PositionQuantityFrom { get; set; }

        [Display(Name = "PtyPositionQuantityTo", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPositionQuantityTo_Required")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIntRange1_1000000")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PositionQuantityTo { get; set; }

        //[Display(Name = "PtyStackeable", ResourceType = typeof(Resources.Resources))]
        //public Boolean Stackable { get; set; }

        [Display(Name = "EtyFreightType", ResourceType = typeof(Resources.Resources))]
        public int FreightTypeID { get; set; }
        [Display(Name = "EtyFreightType", ResourceType = typeof(Resources.Resources))]
        public virtual FreightType FreightType { get; set; }
    }
}
