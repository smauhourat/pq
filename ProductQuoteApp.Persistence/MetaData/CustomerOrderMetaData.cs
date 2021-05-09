using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class CustomerOrderMetaData
    {
        [Display(Name = "PtyCustomerOrderCode", ResourceType = typeof(Resources.Resources))]
        public string CustomerOrderCode { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PtyCustomerOrderDateOrder", ResourceType = typeof(Resources.Resources))]
        public System.DateTime DateOrder { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "PtyCustomerOrderDateOrder", ResourceType = typeof(Resources.Resources))]
        public System.DateTime DateOrderView { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Aprobacion")]
        public DateTime ApprovedDate { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Rechazo")]
        public DateTime RejectedDate { get; set; }

        [Display(Name = "EtyCustomerOrderStatus", ResourceType = typeof(Resources.Resources))]
        public int CustomerOrderStatusID { get; set; }

        [Display(Name = "EtyCustomerOrderStatus", ResourceType = typeof(Resources.Resources))]
        public virtual CustomerOrderStatus CustomerOrderStatus { get; set; }
    }
}