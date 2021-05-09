using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class ProductQuoteMetaData
    {

        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public int CustomerID { get; set; }

        [Display(Name = "EtyProduct", ResourceType = typeof(Resources.Resources))]
        public int ProductID { get; set; }

        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public string SaleModalityName { get; set; }

        [Display(Name = "PtyProductBuyAndSellDirect", ResourceType = typeof(Resources.Resources))]
        public Boolean ProductBuyAndSellDirect { get; set; }

        public string ProductSingleName { get; set; }

        [Display(Name = "PtyValidityOfPrice", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProductValidityOfPrice { get; set; }

        [Display(Name = "PtyInOutStorage", ResourceType = typeof(Resources.Resources))]
        public Boolean ProductInOutStorage { get; set; }

        [Display(Name = "EtyPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        public int PaymentDeadlineID { get; set; }

        [Display(Name = "PtyMaximumMonthsStock", ResourceType = typeof(Resources.Resources))]
        public int MaximumMonthsStock { get; set; }

        [Display(Name = "PtyDeliveryAmount", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyDeliveryAmount_Required")]
        public int DeliveryAmount { get; set; }

        [Display(Name = "PtyQuantity", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Quantity_Required")]
        public int MinimumQuantityDelivery { get; set; }

        [Display(Name = "PtyQuantityOpenPurchaseOrder", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyQuantityOpenPurchaseOrder_Required")]
        public int QuantityOpenPurchaseOrder { get; set; }

        [Display(Name = "PtyPrice", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal CustomerTotalCost { get; set; }

        [Display(Name = "PtyDateQuote", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] //19/10/2018
        public System.DateTime DateQuote { get; set; }

        [Display(Name = "PtyDateDelivery", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? DateDelivery { get; set; }


        //Detalle Costeo

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal CostoProducto_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Flete_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Flete_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal AlmacenamientoCosto_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal AlmacenamientoCosto_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Inout_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Inout_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Merma_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Merma_PorTON { get; set; }
        public decimal CostoFinancieroMensual_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal CostoFinancieroMensual_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal CostoFin_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GastosFijos_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GastosFijos_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal ImpuestoDC_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal ImpuestoDC_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal IIBBAlicuota_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal IIBBAlicuota_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Comisiones_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal Comisiones_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoUSD_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoPorc_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public string MargenNetoEntidadOrigen { get; set; }

        [Display(Name = "PtyPriceOriginal", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal PriceOriginal { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoOriginalUSD_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoOriginalPorc_PorTON { get; set; }

        [Display(Name = "PtyObservations", ResourceType = typeof(Resources.Resources))]
        public string Observations { get; set; }

        [Display(Name = "PtyObservations", ResourceType = typeof(Resources.Resources))]
        public string UserObservations { get; set; }

        [Display(Name = "PtyTiempoMedioStockMeses", ResourceType = typeof(Resources.Resources))]
        public decimal TiempoMedioStockMeses { get; set; }


        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        [Display(Name = "PtyCostoFinancieroMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_CostoFinancieroMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        [Display(Name = "PtyCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_CostoAlmacenamientoMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        [Display(Name = "PtyFactorCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_FactorCostoAlmacenamientoMensual { get; set; }

        [Display(Name = "PtyDiasStockPromedioDistLocal", ResourceType = typeof(Resources.Resources))]
        public int GV_DiasStockPromedioDistLocal { get; set; }

        [Display(Name = "PtyFreightType", ResourceType = typeof(Resources.Resources))]
        public string FreightType { get; set; }


        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal PrecioVentaRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoUSDRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal MargenNetoPORCRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal WorkingCapital { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_CostoAlmacenamientoMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_CostoInOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_CostoFinancieroMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_ImpuestoDebitoCredito { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_GastosFijos { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_IIBBAlicuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_TipoCambio { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal GVD_FactorCostoAlmacenamientoMensual { get; set; }

        public int GVD_DiasStockPromedioDistLocal { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal? CostoFleteInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal? PrecioInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal? MargenInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal? MargenUSDInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal? SellerCommissionInput { get; set; }

        [Display(Name = "PtyValidityOfPrice", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProductValidityOfPriceInput { get; set; }

        public Boolean IsSellerUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}")]
        public decimal? FactorRofexInput { get; set; }

        public string DeliveryAddress { get; set; }

        [Display(Name = "PtyAvailabilityDays", ResourceType = typeof(Resources.Resources))]
        public int? AvailabilityDays { get; set; }

        [Display(Name = "PtyDatesDeliveryInput", ResourceType = typeof(Resources.Resources))]
        public string DatesDeliveryInput { get; set; }

        public string ProductSellerCompanyName { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]

        [Display(Name = "PtyExchangeInsurance", ResourceType = typeof(Resources.Resources))]
        public decimal ExchangeInsurance { get; set; }

        public string ProductQuotePDFFooter { get; set; }
    }
}
