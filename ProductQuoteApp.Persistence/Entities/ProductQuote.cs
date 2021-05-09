using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Persistence
{
    public enum ProductQuoteStatus
    {
        Inicial, //Cuando se realiza
        Creada,
        Procesada
    }        

    [MetadataType(typeof(ProductQuoteMetaData))]
    public class ProductQuote : ICloneable
    {
        public int ProductQuoteID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ProductQuoteCode { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public string CustomerContactMail { get; set; }
        public string CustomerCompany { get; set; }        
        public int CustomerDelayAverageDays { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductProviderName { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductFCLKilogram { get; set; }
        public decimal ProductWaste { get; set; }
        public int ProductProviderPaymentDeadline { get; set; }
        public int ProductLeadTime { get; set; }
        public Boolean ProductBuyAndSellDirect { get; set; }
        public string ProductSingleName { get; set; }
        public int ProductPackagingID { get; set; }
        public string ProductPackagingName { get; set; }
        public DateTime? ProductValidityOfPrice { get; set; }

        public Boolean ProductInOutStorage { get; set; }

        public int SaleModalityID { get; set; }
        public string SaleModalityName { get; set; }
        public int DeliveryAmount { get; set; }
        public int MinimumQuantityDelivery { get; set; }
        public int QuantityOpenPurchaseOrder { get; set; }
        public int? MaximumMonthsStock { get; set; }
        public int GeographicAreaID { get; set; }
        public string GeographicAreaName { get; set; }
        public int PaymentDeadlineID { get; set; }
        [Precision(25, 10)]
        public decimal PaymentDeadlineMonths { get; set; }
        public string PaymentDeadlineName { get; set; }

        public int ExchangeTypeID { get; set; }
        public string ExchangeTypeName { get; set; }
        public string ExchangeTypeLargeDescription { get; set; }

        [Precision(25, 10)]
        public decimal Price { get; set; }

        [Precision(25, 10)]
        public decimal CustomerTotalCost { get; set; }

        [Precision(25, 10)]
        public decimal TypeChange { get; set; }


        [Precision(25, 10)]
        public decimal CostoProducto_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal Flete_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal Flete_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal Inout_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal Inout_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal Merma_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal Merma_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal CostoFin_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal GastosFijos_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal GastosFijos_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal ImpuestoDC_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal ImpuestoDC_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal Comisiones_PorITEM { get; set; }

        [Precision(25, 10)]
        public decimal Comisiones_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoUSD_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoPorc_PorTON { get; set; }
        public string MargenNetoEntidadOrigen { get; set; }

        [Precision(25, 10)]
        public decimal PriceOriginal { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoOriginalUSD_PorTON { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoOriginalPorc_PorTON { get; set; }

        public DateTime DateQuote { get; set; }
        public DateTime? DateDelivery { get; set; }

        public string LeyendaCalculoCostoTransporte { get; set; }
        public string ProductQuotePDF { get; set; }

        public string Message { get; set; }

        public string Observations { get; set; }

        public string UserObservations { get; set; }
        

        [Precision(25, 10)]
        public decimal FactorRofex { get; set; }

        [Precision(25, 10)]
        public decimal ValorRofex { get; set; }        

        [Precision(25, 10)]
        public decimal TiempoMedioStockMeses { get; set; }


        [Precision(25, 10)]
        public decimal GV_CostoFinancieroMensual { get; set; }
        [Precision(25, 10)]
        public decimal GV_CostoAlmacenamientoMensual { get; set; }
        [Precision(25, 10)]
        public decimal GV_FactorCostoAlmacenamientoMensual { get; set; }
        
        public int GV_DiasStockPromedioDistLocal { get; set; }

        public string FreightType { get; set; }

        public string UserId { get; set; }
        public virtual AdminUser User { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }

        public int? DueDateReasonID { get; set; }
        public virtual DueDateReason DueDateReason { get; set; }

        [Precision(25, 10)]
        public decimal PrecioVentaRofex { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoUSDRofex { get; set; }

        [Precision(25, 10)]
        public decimal MargenNetoPORCRofex { get; set; }

        [Precision(25, 10)]
        public decimal WorkingCapital { get; set; }

        public virtual ShipmentTracking ShipmentTracking { get; set; }
        [NotMapped]
        public bool CanShowCosteo { get; set; }
        public string CostoFinancieroMensual_PorITEM_Formula { get; set; }
        public string WorkingCapital_Formula { get; set; }
        public string TiempoMedioStockDias_Formula { get; set; }

        [Precision(25, 10)]
        public decimal GVD_CostoAlmacenamientoMensual { get; set; }

        [Precision(25, 10)]
        public decimal GVD_CostoInOut { get; set; }

        [Precision(25, 10)]
        public decimal GVD_CostoFinancieroMensual { get; set; }

        [Precision(25, 10)]
        public decimal GVD_ImpuestoDebitoCredito { get; set; }

        [Precision(25, 10)]
        public decimal GVD_GastosFijos { get; set; }

        [Precision(25, 10)]
        [Obsolete("No usar, utilizar el porcentaje de IIBB de cada producto, que se aplica sobre el precio de venta para los casos Fabricación y Reventa y sobre (precio de venta - precio de compra) para el caso de cuenta y orden")]
        public decimal GVD_IIBBAlicuota { get; set; }

        [Precision(25, 10)]
        public decimal GVD_TipoCambio { get; set; }

        [Precision(25, 10)]
        public decimal GVD_FactorCostoAlmacenamientoMensual { get; set; }

        public int GVD_DiasStockPromedioDistLocal { get; set; }

        [Precision(25, 10)]
        public decimal? CostoFleteInput { get; set; }

        [Precision(25, 10)]
        public decimal? PrecioInput { get; set; }

        [Precision(25, 10)]
        public decimal? MargenInput { get; set; }

        [Precision(25, 10)]
        public decimal? MargenUSDInput { get; set; }

        [Precision(25, 10)]
        public decimal? SellerCommissionInput { get; set; }

        public DateTime? ProductValidityOfPriceInput { get; set; }

        public Boolean IsSellerUser { get; set; }

        [Precision(25, 10)]
        public decimal? FactorRofexInput { get; set; }

        public string DeliveryAddress { get; set; }

        public int? AvailabilityDays { get; set; }

        public string DatesDeliveryInput { get; set; }

        public string ProductSellerCompanyName { get; set; }

        [Precision(25, 10)]
        public decimal ExchangeInsurance { get; set; }

        [Precision(25, 10)]
        public decimal ExchangeInsurance_PorTON { get; set; }

        public bool HasWayOfException { get; set; }

        [Precision(25, 10)]
        public decimal WayOfExceptionValue { get; set; }

        public ExceptionParamType IsSaleModalityFindParam { get; set; }
        public ExceptionParamType IsGeographicAreaFindParam { get; set; }
        public ExceptionParamType IsPaymentDeadlineFindParam { get; set; }
        public ExceptionParamType IsQuantityOpenPurchaseOrderFindParam { get; set; }
        public ExceptionParamType IsDeliveryAmountFindParam { get; set; }
        public ExceptionParamType IsMaximumMonthsStockFindParam { get; set; }
        public ExceptionParamType IsExchangeTypeFindParam { get; set; }
        public ExceptionParamType IsMinimumQuantityDeliveryFindParam { get; set; }

        public ExceptionApplyType ExceptionApplyType { get; set; }

        public string UserFullName { get; set; }
        public string ProductQuotePDFFooter { get; set; }
        public string ProductQuoteSmallPDFFooter { get; set; }

        public int? ContactTypeID { get; set; }
        public virtual ContactType ContactType { get; set; }

        public DateTime? ClosureDate { get; set; }

        public int? ReasonsForClosureID { get; set; }
        public virtual ReasonsForClosure ReasonsForClosure { get; set; }

        public string ClosureObservations { get; set; }

        public DateTime? DateSent { get; set; }

        public string ProductQuoteSmallPDF { get; set; }

        public Boolean ExpressCalc { get; set; }

        public int SalesChannelID { get; set; }
        public virtual SalesChannel SalesChannel { get; set; }

        [NotMapped]
        public string ProductQuoteStatus
        {
            get
            {
                if (this.CustomerOrder == null)
                {
                    if (this.DateSent != null)
                    {
                        return "Enviada";
                    }
                    else
                    {
                        if (this.ReasonsForClosureID != null)
                        {
                            return "Cerrada";
                        }
                        else
                        {
                            return "Abierta";
                        }
                    }
                }
                else
                {
                    return this.CustomerOrder.CustomerOrderStatus.Description;
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
