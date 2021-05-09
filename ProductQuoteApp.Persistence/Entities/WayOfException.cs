using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ProductQuoteApp.Persistence
{
    public enum ExceptionParamType
    {
        [Description("Parametro Indefinido")]
        Indefinido = 0,
        [Description("Parametro Variable")]
        ParametroVariable = 1,
        [Description("Parametro Encontrado")]
        ParametroEncontrado = 2,
        [Description("Parametro No Encontrado")]
        ParametroNoEncontrado = 3,
    }

    public enum ExceptionApplyType
    {
        [Description("Precio Proporcional")]
        ApplyProporcionalPrice = 1,
        [Description("Margen Equivalente")]
        ApplyEquivalentMargin = 2,
        [Description("Precio Fijo")]
        ApplyPriceDirect = 3
    }

    [MetadataType(typeof(WayOfExceptionMetaData))]
    [CustomValidation(typeof(WayOfExceptionRules), "ValidateWayOfException")]
    public class WayOfException
    {
        public int WayOfExceptionID { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int SaleModalityID { get; set; }
        public virtual SaleModality SaleModality { get; set; }
        public Boolean IsSaleModalitySearchParam { get; set; }
        public int GeographicAreaID { get; set; }
        public virtual GeographicArea GeographicArea { get; set; }
        public Boolean IsGeographicAreaSearchParam { get; set; }
        public int PaymentDeadlineID { get; set; }
        public virtual PaymentDeadline PaymentDeadline { get; set; }
        public Boolean IsPaymentDeadlineSearchParam { get; set; }
        public int QuantityOpenPurchaseOrder { get; set; }
        public Boolean IsQuantityOpenPurchaseOrderSearchParam { get; set; }
        public int DeliveryAmount { get; set; }
        public Boolean IsDeliveryAmountSearchParam { get; set; }
        public int MaximumMonthsStock { get; set; }
        public Boolean IsMaximumMonthsStockSearchParam { get; set; }
        public int ExchangeTypeID { get; set; }
        public virtual ExchangeType ExchangeType { get; set; }
        public Boolean IsExchangeTypeSearchParam { get; set; }

        [Precision(25, 10)]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal ExceptionPrice { get; set; }

        public ExceptionApplyType ExceptionApplyType { get; set; }

        [NotMapped]
        public decimal ExceptionMargin { get; set; }

        [NotMapped]
        public decimal ExceptionPriceFactor { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal? MinimumQuantityDelivery { get
            {
                if ((decimal)this.DeliveryAmount > 0)
                    return this.QuantityOpenPurchaseOrder / (decimal)this.DeliveryAmount;
                return 0;
            }
        }

        public Boolean IsMinimumQuantityDeliverySearchParam { get; set; }
    }
}
