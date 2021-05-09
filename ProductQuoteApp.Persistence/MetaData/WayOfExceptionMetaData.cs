using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class WayOfExceptionMetaData
    {
        [Required]
        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public int CustomerID { get; set; }

        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public virtual Customer Customer { get; set; }

        [Required]
        [Display(Name = "EtyProduct", ResourceType = typeof(Resources.Resources))]
        public int ProductID { get; set; }

        [Display(Name = "EtyProduct", ResourceType = typeof(Resources.Resources))]
        public virtual Product Product { get; set; }

        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public int SaleModalityID { get; set; }

        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public virtual SaleModality SaleModality { get; set; }

        [Display(Name = "PtyIsSaleModalitySearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsSaleModalitySearchParam { get; set; }


        [Display(Name = "EtyGeographicArea", ResourceType = typeof(Resources.Resources))]
        public int GeographicAreaID { get; set; }

        [Display(Name = "EtyGeographicArea", ResourceType = typeof(Resources.Resources))]
        public virtual GeographicArea GeographicArea { get; set; }

        [Display(Name = "PtyIsGeographicAreaSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsGeographicAreaSearchParam { get; set; }

        [Display(Name = "EtyPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        public int PaymentDeadlineID { get; set; }

        [Display(Name = "EtyPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        public virtual PaymentDeadline PaymentDeadline { get; set; }

        [Display(Name = "PtyIsPaymentDeadlineSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsPaymentDeadlineSearchParam { get; set; }
        
        [Display(Name = "PtyQuantityOpenPurchaseOrder", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int QuantityOpenPurchaseOrder { get; set; }

        [Display(Name = "PtyIsQuantityOpenPurchaseOrderSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsQuantityOpenPurchaseOrderSearchParam { get; set; }
        
        [Display(Name = "PtyDeliveryAmount", ResourceType = typeof(Resources.Resources))]
        [Required, Range(1, 12, ErrorMessage = "Debe seleccionar la Cantidad de Entregas")]
        public int DeliveryAmount { get; set; }

        [Display(Name = "PtyIsDeliveryAmountSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsDeliveryAmountSearchParam { get; set; }
        
        [Display(Name = "PtyMaximumMonthsStock", ResourceType = typeof(Resources.Resources))]
        public int MaximumMonthsStock { get; set; }

        [Display(Name = "PtyIsMaximumMonthsStockSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsMaximumMonthsStockSearchParam { get; set; }

        [Display(Name = "PtyExchangeType", ResourceType = typeof(Resources.Resources))]
        public int ExchangeTypeID { get; set; }

        [Display(Name = "PtyExchangeType", ResourceType = typeof(Resources.Resources))]
        public virtual ExchangeType ExchangeType { get; set; }

        [Display(Name = "PtyIsExchangeTypeSearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsExchangeTypeSearchParam { get; set; }

        [Display(Name = "PtyExceptionPrice", ResourceType = typeof(Resources.Resources))]
        [Precision(25, 10)]
        //[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal ExceptionPrice { get; set; }

        //[Display(Name = "PtyApplyPriceFactor", ResourceType = typeof(Resources.Resources))]
        //public Boolean ApplyPriceFactor { get; set; }

        [Display(Name = "PtyIsMinimumQuantityDeliverySearchParam", ResourceType = typeof(Resources.Resources))]
        public Boolean IsMinimumQuantityDeliverySearchParam { get; set; }
    }
}
