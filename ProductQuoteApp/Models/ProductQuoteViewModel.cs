using ProductQuoteApp.Library;
using ProductQuoteApp.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Models
{
    public class ProductQuoteViewModel
    {
        public int ProductQuoteID { get; set; }

        public string ProductQuoteCode { get; set; }

        [Display(Name = "EtyCustomer", ResourceType = typeof(Resources.Resources))]
        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerContactName { get; set; }
        public string CustomerContactMail { get; set; }
        public string CustomerCompany { get; set; }

        public int CustomerDelayAverageDays { get; set; }

        [Display(Name = "EtyProduct", ResourceType = typeof(Resources.Resources))]
        public int ProductID { get; set; }

        [Display(Name = "EtyProduct", ResourceType = typeof(Resources.Resources))]
        public string ProductName { get; set; }
        public string ProductProviderName { get; set; }
        public int ProductPackagingID { get; set; }
        public string ProductPackagingName { get; set; }

        [Display(Name = "PtyValidityOfPrice", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProductValidityOfPrice { get; set; }

        public decimal ProductWaste { get; set; }
        public int ProductProviderPaymentDeadline { get; set; }
        public int ProductLeadTime { get; set; }

        public Boolean ProductBuyAndSellDirect { get; set; }
        public string ProductBuyAndSellDirect_Desc { get { return ProductBuyAndSellDirect ? "SI" : "NO"; } }

        public Boolean ProductInOutStorage { get; set; }
        public string ProductInOutStorage_Desc { get { return ProductInOutStorage ? "SI" : "NO"; } }

        public string ProductSingleName { get; set; }

        public string ProductFCLKilogram { get; set; }
        public int SaleModalityID { get; set; }

        [Display(Name = "EtySaleModality", ResourceType = typeof(Resources.Resources))]
        public string SaleModalityName { get; set; }

        [Display(Name = "PtyDeliveryAmount", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int DeliveryAmount { get; set; }

        [Display(Name = "PtyQuantity", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int MinimumQuantityDelivery { get; set; }

        [Display(Name = "PtyQuantityOpenPurchaseOrder", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int QuantityOpenPurchaseOrder { get; set; }

        [Display(Name = "PtyMaximumMonthsStock", ResourceType = typeof(Resources.Resources))]
        public int? MaximumMonthsStock { get; set; }

        [Display(Name = "EtyGeographicArea", ResourceType = typeof(Resources.Resources))]
        public int GeographicAreaID { get; set; }

        [Display(Name = "EtyGeographicArea", ResourceType = typeof(Resources.Resources))]
        public string GeographicAreaName { get; set; }

        [Display(Name = "EtyPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        public int PaymentDeadlineID { get; set; }

        public decimal PaymentDeadlineMonths { get; set; }

        [Display(Name = "EtyPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        public string PaymentDeadlineName { get; set; }

        public int ExchangeTypeID { get; set; }
        [Display(Name = "PtyExchangeType", ResourceType = typeof(Resources.Resources))]
        public string ExchangeTypeName { get; set; }
        [Display(Name = "PtyExchangeType", ResourceType = typeof(Resources.Resources))]
        public string ExchangeTypeLargeDescription { get; set; }

        public bool CanShowCosteo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]

        [Display(Name = "PtyPrice", ResourceType = typeof(Resources.Resources))]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Precision(25, 10)]
        [Display(Name = "PtyPrice", ResourceType = typeof(Resources.Resources))]        
        public decimal Price_View { get { return Helper.RoundDecimal(this.Price, 3); } }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal CustomerTotalCost { get; set; }

        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Precision(25, 10)]
        public decimal TypeChange { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal CostoProducto_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Flete_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Flete_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal Inout_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Inout_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Merma_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Merma_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal CostoFin_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal GastosFijos_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GastosFijos_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ImpuestoDC_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal ImpuestoDC_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Comisiones_PorITEM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Comisiones_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSD_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoPorc_PorTON { get; set; }
        public string MargenNetoEntidadOrigen { get; set; }

        [Display(Name = "PtyDateQuote", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateQuote { get; set; }
        
        [Display(Name = "PtyDateDelivery", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateDelivery { get; set; }

        public string LeyendaCalculoCostoTransporte { get; set; }
        public string ProductQuotePDF { get; set; }
        public string ProductQuoteSmallPDF { get; set; }

        public string Message { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }

        public int? DueDateReasonID { get; set; }
        public virtual DueDateReason DueDateReason { get; set; }

        public int? ReasonsForClosureID { get; set; }
        public virtual ReasonsForClosure ReasonsForClosure { get; set; }

        [Display(Name = "PtyClosureDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ClosureDate { get; set; }

        [Display(Name = "PtyClosureObservations", ResourceType = typeof(Resources.Resources))]
        public string ClosureObservations { get; set; }

        [Display(Name = "PtyDateSent", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateSent { get; set; }


        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Price_PorOC { get; private set; }

        [Display(Name = "PtyPrice_PorOC", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Price_PorOC_View { get { return Helper.RoundDecimal(this.Price_PorOC, 2); } }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal Price_PorEntrega { get; private set; }


        [Display(Name = "PtyPrice_PorEntrega", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Price_PorEntrega_View { get { return Helper.RoundDecimal(this.Price_PorEntrega, 2); } }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Flete_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Flete_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorEntrega { get; private set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoCosto_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Inout_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Inout_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Merma_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Merma_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal CostoFinancieroMensual_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal GastosFijos_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal GastosFijos_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ImpuestoDC_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ImpuestoDC_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal IIBBAlicuota_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Comisiones_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Comisiones_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal CostoProducto_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal CostoProducto_PorOC { get; private set; }

        //ojo, se quitaron decimales (10/09/2020)
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        //[DataType(DataType.Currency)]
        public decimal CustomerTotalCost_PorEntrega { get; private set; }

        //ojo, se quitaron decimales (10/09/2020)
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal CustomerTotalCost_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSD_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSD_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoPorc_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal MargenNetoPorc_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        [Precision(25, 10)]
        public decimal FactorRofex { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Precision(25, 10)]
        public decimal ValorRofex { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal TiempoMedioStockMeses { get; private set; }
        public string UserId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal PriceOriginal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal PriceOriginal_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal PriceOriginal_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalUSD_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalUSD_PorEntrega { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalUSD_PorOC { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalPorc_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalPorc_PorEntrega { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoOriginalPorc_PorOC { get; set; }

        [Display(Name = "PtyObservations", ResourceType = typeof(Resources.Resources))]
        public string Observations { get; set; }

        [Display(Name = "PtyObservations", ResourceType = typeof(Resources.Resources))]
        public string UserObservations { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        [Display(Name = "PtyCostoFinancieroMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_CostoFinancieroMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        [Display(Name = "PtyCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_CostoAlmacenamientoMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        [Display(Name = "PtyFactorCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        public decimal GV_FactorCostoAlmacenamientoMensual { get; set; }

        [Display(Name = "PtyDiasStockPromedioDistLocal", ResourceType = typeof(Resources.Resources))]
        public int GV_DiasStockPromedioDistLocal { get; set; }

        [Display(Name = "PtyFreightType", ResourceType = typeof(Resources.Resources))]
        public string FreightType { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal PrecioVentaRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal PrecioVentaRofex_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal PrecioVentaRofex_PorOC { get; private set; }


        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSDRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoPORCRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSDRofex_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenNetoUSDRofex_PorOC { get; private set; }

        [Precision(25, 10)]
        public decimal MargenNetoPORCRofex_PorEntrega { get; private set; }

        [Precision(25, 10)]
        public decimal MargenNetoPORCRofex_PorOC { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal MargenBrutoPORCRofex { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal WorkingCapitalRotation { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal WorkingCapitalROI { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Flete_PorITEMUSD { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal TiempoMedioStockDias { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public int? MaximoTiempoStockDias { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal WorkingCapital { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal PaymentDeadlineDays { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoARS { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoUSD { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        [Precision(25, 10)]
        public decimal AlmacenamientoUSDKg { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal Inout_USD { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        [Precision(25, 10)]
        public decimal Inout_USDKg { get; private set; }

        public string CostoFinancieroMensual_PorITEM_Formula { get; set; }
        public string WorkingCapital_Formula { get; set; }
        public string TiempoMedioStockDias_Formula { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_CostoAlmacenamientoMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_CostoInOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_CostoFinancieroMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_ImpuestoDebitoCredito { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_GastosFijos { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_IIBBAlicuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_TipoCambio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal GVD_FactorCostoAlmacenamientoMensual { get; set; }

        public int GVD_DiasStockPromedioDistLocal { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal? CostoFleteInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal? PrecioInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal? MargenInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal? MargenUSDInput { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2")]
        [Precision(25, 10)]
        public decimal? SellerCommissionInput { get; set; }

        [Display(Name = "PtyValidityOfPrice", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ProductValidityOfPriceInput { get; set; }

        public Boolean IsSellerUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal? FactorRofexInput { get; set; }

        public string DeliveryAddress { get; set; }

        [Display(Name = "PtyAvailabilityDays", ResourceType = typeof(Resources.Resources))]
        public int? AvailabilityDays { get; set; }

        [Display(Name = "PtyDatesDeliveryInput", ResourceType = typeof(Resources.Resources))]
        public string DatesDeliveryInput { get; set; }

        [Display(Name = "EtySellerCompany", ResourceType = typeof(Resources.Resources))]
        public string ProductSellerCompanyName { get; set; }

        [Display(Name = "PtyExchangeInsurance", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ExchangeInsurance { get; set; }


        [DisplayFormat(DataFormatString = "{0:N5}")]
        [Precision(25, 10)]
        public decimal ExchangeInsurance_PorTON { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ExchangeInsurance_PorEntrega { get; private set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Precision(25, 10)]
        public decimal ExchangeInsurance_PorOC { get; private set; }

        public bool HasWayOfException { get; set; }

        public string HasWayOfException_View {
            get {
                if (HasWayOfException)
                    return "SI";
                else
                    return "NO";
            }
        }

        public ExceptionApplyType ExceptionApplyType { get; set; }

        [DisplayFormat(DataFormatString = "{0:N4}")]
        [Precision(25, 10)]
        public decimal WayOfExceptionValue { get; set; }

        public string CaptionWayOfException
        {
            get {
                if (HasWayOfException)
                    return "SI - " + this.ExceptionApplyType.Description() + " - " + WayOfExceptionValue.ToString("0.0000");
                else
                    return "NO";
            }
        }

        public ExceptionParamType IsSaleModalityFindParam { get; set; }
        public string IsSaleModalityFindParam_View {
            get {
                if (HasWayOfException)
                    return IsSaleModalityFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsGeographicAreaFindParam { get; set; }
        public string IsGeographicAreaFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsGeographicAreaFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsPaymentDeadlineFindParam { get; set; }
        public string IsPaymentDeadlineFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsPaymentDeadlineFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsQuantityOpenPurchaseOrderFindParam { get; set; }
        public string IsQuantityOpenPurchaseOrderFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsQuantityOpenPurchaseOrderFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsDeliveryAmountFindParam { get; set; }
        public string IsDeliveryAmountFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsDeliveryAmountFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsMaximumMonthsStockFindParam { get; set; }
        public string IsMaximumMonthsStockFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsMaximumMonthsStockFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsExchangeTypeFindParam { get; set; }
        public string IsExchangeTypeFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsExchangeTypeFindParam.ToString();
                return string.Empty;
            }
        }
        public ExceptionParamType IsMinimumQuantityDeliveryFindParam { get; set; }
        public string IsMinimumQuantityDeliveryFindParam_View
        {
            get
            {
                if (HasWayOfException)
                    return IsMinimumQuantityDeliveryFindParam.ToString();
                return string.Empty;
            }
        }

        [NotMapped]
        public string ProductQuoteStatus { get;set; }

        public string UserFullName { get; set; }
        public string UserInitials { get; set; }
        public string ProductQuotePDFFooter { get; set; }
        public string ProductQuoteSmallPDFFooter { get; set; }
        public Boolean ExpressCalc { get; set; }

        public ProductQuoteViewModel()
        { }

        public ProductQuoteViewModel(ProductQuote pq)
        {
            this.ProductQuoteID = pq.ProductQuoteID;
            this.ProductQuoteCode = pq.ProductQuoteCode;
            this.CustomerID = pq.CustomerID;
            this.CustomerName = pq.CustomerName;
            this.CustomerContactName = pq.CustomerContactName;
            this.CustomerContactMail = pq.CustomerContactMail;
            this.CustomerCompany = pq.CustomerCompany;
            this.CustomerDelayAverageDays = pq.CustomerDelayAverageDays;
            this.ProductID = pq.ProductID;
            this.ProductName = pq.ProductName;
            this.ProductProviderName = pq.ProductProviderName;
            this.ProductPackagingID = pq.ProductPackagingID;
            this.ProductPackagingName = pq.ProductPackagingName;
            this.ProductValidityOfPrice = pq.ProductValidityOfPrice;
            this.ProductWaste = pq.ProductWaste;
            this.ProductProviderPaymentDeadline = pq.ProductProviderPaymentDeadline;
            this.ProductLeadTime = pq.ProductLeadTime;
            this.ProductBuyAndSellDirect = pq.ProductBuyAndSellDirect;
            this.ProductInOutStorage = pq.ProductInOutStorage;
            this.ProductSingleName = pq.ProductSingleName;
            this.ProductSellerCompanyName = pq.ProductSellerCompanyName;
            this.ProductFCLKilogram = pq.ProductFCLKilogram;
            this.SaleModalityID = pq.SaleModalityID;
            this.SaleModalityName = pq.SaleModalityName;
            this.DeliveryAmount = pq.DeliveryAmount;
            this.MinimumQuantityDelivery = pq.MinimumQuantityDelivery;
            this.QuantityOpenPurchaseOrder = pq.QuantityOpenPurchaseOrder;
            this.MaximumMonthsStock = pq.MaximumMonthsStock;
            this.GeographicAreaID = pq.GeographicAreaID;
            this.GeographicAreaName = pq.GeographicAreaName;
            this.PaymentDeadlineID = pq.PaymentDeadlineID;
            this.PaymentDeadlineMonths = pq.PaymentDeadlineMonths;
            this.PaymentDeadlineName = pq.PaymentDeadlineName;
            this.ExchangeTypeID = pq.ExchangeTypeID;
            this.ExchangeTypeName = pq.ExchangeTypeName;
            this.ExchangeTypeLargeDescription = pq.ExchangeTypeLargeDescription;
            this.CanShowCosteo = pq.CanShowCosteo;

            this.Price = pq.Price;
            this.Price_PorEntrega = this.Price * this.MinimumQuantityDelivery;
            this.Price_PorOC = this.Price * this.QuantityOpenPurchaseOrder;

            this.PriceOriginal = pq.PriceOriginal;
            this.PriceOriginal_PorEntrega = this.PriceOriginal * this.MinimumQuantityDelivery;
            this.PriceOriginal_PorOC = this.PriceOriginal * this.QuantityOpenPurchaseOrder;

            this.CustomerTotalCost = pq.CustomerTotalCost;

            this.TypeChange = pq.TypeChange;

            this.CostoProducto_PorTON = pq.CostoProducto_PorTON;
            this.CostoProducto_PorEntrega = pq.CostoProducto_PorTON * pq.MinimumQuantityDelivery;
            this.CostoProducto_PorOC = pq.CostoProducto_PorTON * pq.QuantityOpenPurchaseOrder;

            this.Flete_PorITEM = pq.Flete_PorITEM;
            this.Flete_PorTON = pq.Flete_PorTON;
            this.Flete_PorEntrega = pq.Flete_PorTON * pq.MinimumQuantityDelivery;
            this.Flete_PorOC = pq.Flete_PorTON * pq.QuantityOpenPurchaseOrder;

            this.AlmacenamientoCosto_PorITEM = pq.AlmacenamientoCosto_PorITEM;
            this.AlmacenamientoCosto_PorTON = pq.AlmacenamientoCosto_PorTON;
            this.AlmacenamientoCosto_PorEntrega = pq.AlmacenamientoCosto_PorTON * pq.MinimumQuantityDelivery;
            this.AlmacenamientoCosto_PorOC = pq.AlmacenamientoCosto_PorTON * pq.QuantityOpenPurchaseOrder;

            this.Inout_PorITEM = pq.Inout_PorITEM;
            this.Inout_PorTON = pq.Inout_PorTON;
            this.Inout_PorEntrega = pq.Inout_PorTON * pq.MinimumQuantityDelivery;
            this.Inout_PorOC = pq.Inout_PorTON * pq.QuantityOpenPurchaseOrder;

            this.Merma_PorITEM = pq.Merma_PorITEM;
            this.Merma_PorTON = pq.Merma_PorTON;
            this.Merma_PorEntrega = pq.Merma_PorTON * pq.MinimumQuantityDelivery;
            this.Merma_PorOC = pq.Merma_PorTON * pq.QuantityOpenPurchaseOrder;

            this.CostoFinancieroMensual_PorITEM = pq.CostoFinancieroMensual_PorITEM;
            this.CostoFinancieroMensual_PorTON = pq.CostoFinancieroMensual_PorTON;
            this.CostoFinancieroMensual_PorEntrega = pq.CostoFinancieroMensual_PorTON * pq.MinimumQuantityDelivery;
            this.CostoFinancieroMensual_PorOC = pq.CostoFinancieroMensual_PorTON * pq.QuantityOpenPurchaseOrder;

            this.ExchangeInsurance = pq.ExchangeInsurance;
            this.ExchangeInsurance_PorTON = pq.ExchangeInsurance_PorTON;
            this.ExchangeInsurance_PorEntrega = pq.ExchangeInsurance_PorTON * pq.MinimumQuantityDelivery;
            this.ExchangeInsurance_PorOC = pq.ExchangeInsurance_PorTON * pq.QuantityOpenPurchaseOrder;

            this.GastosFijos_PorITEM = pq.GastosFijos_PorITEM;
            this.GastosFijos_PorTON = pq.GastosFijos_PorTON;
            this.GastosFijos_PorEntrega = pq.GastosFijos_PorTON * pq.MinimumQuantityDelivery;
            this.GastosFijos_PorOC = pq.GastosFijos_PorTON * pq.QuantityOpenPurchaseOrder;

            this.ImpuestoDC_PorITEM = pq.ImpuestoDC_PorITEM;
            this.ImpuestoDC_PorTON = pq.ImpuestoDC_PorTON;
            this.ImpuestoDC_PorEntrega = pq.ImpuestoDC_PorTON * pq.MinimumQuantityDelivery;
            this.ImpuestoDC_PorOC = pq.ImpuestoDC_PorTON * pq.QuantityOpenPurchaseOrder;

            this.IIBBAlicuota_PorITEM = pq.IIBBAlicuota_PorITEM;
            this.IIBBAlicuota_PorTON = pq.IIBBAlicuota_PorTON;
            this.IIBBAlicuota_PorEntrega = pq.IIBBAlicuota_PorTON * pq.MinimumQuantityDelivery;
            this.IIBBAlicuota_PorOC = pq.IIBBAlicuota_PorTON * pq.QuantityOpenPurchaseOrder;

            this.Comisiones_PorITEM = pq.Comisiones_PorITEM;
            this.Comisiones_PorTON = pq.Comisiones_PorTON;
            this.Comisiones_PorEntrega = pq.Comisiones_PorTON * pq.MinimumQuantityDelivery;
            this.Comisiones_PorOC = pq.Comisiones_PorTON * pq.QuantityOpenPurchaseOrder;

            this.CustomerTotalCost_PorEntrega = this.CostoProducto_PorEntrega + this.Flete_PorEntrega + this.AlmacenamientoCosto_PorEntrega + this.Inout_PorEntrega + this.Merma_PorEntrega + this.CostoFinancieroMensual_PorEntrega + this.ExchangeInsurance_PorEntrega + this.GastosFijos_PorEntrega + this.ImpuestoDC_PorEntrega + this.IIBBAlicuota_PorEntrega + this.Comisiones_PorEntrega;
            this.CustomerTotalCost_PorOC = this.CostoProducto_PorOC + this.Flete_PorOC + this.AlmacenamientoCosto_PorOC + this.Inout_PorOC + this.Merma_PorOC + this.CostoFinancieroMensual_PorOC + this.ExchangeInsurance_PorOC + this.GastosFijos_PorOC + this.ImpuestoDC_PorOC + this.IIBBAlicuota_PorOC + this.Comisiones_PorOC;

            this.MargenNetoUSD_PorTON = pq.MargenNetoUSD_PorTON;
            this.MargenNetoUSD_PorEntrega = this.Price_PorEntrega - this.CustomerTotalCost_PorEntrega;
            this.MargenNetoUSD_PorOC = this.Price_PorOC - this.CustomerTotalCost_PorOC;

            this.MargenNetoOriginalUSD_PorTON = pq.MargenNetoOriginalUSD_PorTON;
            this.MargenNetoOriginalUSD_PorEntrega = this.PriceOriginal_PorEntrega - this.CustomerTotalCost_PorEntrega;
            this.MargenNetoOriginalUSD_PorOC = this.PriceOriginal_PorOC - this.CustomerTotalCost_PorOC;


            this.MargenNetoPorc_PorTON = pq.MargenNetoPorc_PorTON;
            this.MargenNetoPorc_PorEntrega = (this.MargenNetoUSD_PorEntrega / this.Price_PorEntrega) * 100;
            this.MargenNetoPorc_PorOC = (this.MargenNetoUSD_PorOC / this.Price_PorOC) * 100;

            this.MargenNetoOriginalPorc_PorTON = pq.MargenNetoOriginalPorc_PorTON;
            this.MargenNetoOriginalPorc_PorEntrega = (this.MargenNetoOriginalUSD_PorEntrega / this.PriceOriginal_PorEntrega) * 100;
            this.MargenNetoOriginalPorc_PorOC = (this.MargenNetoOriginalUSD_PorOC / this.PriceOriginal_PorOC) * 100;

            this.FactorRofex = pq.FactorRofex;
            this.ValorRofex = pq.ValorRofex;

            this.MargenNetoEntidadOrigen = pq.MargenNetoEntidadOrigen;
            this.DateQuote = pq.DateQuote;
            this.DateDelivery = pq.DateDelivery;
            this.LeyendaCalculoCostoTransporte = pq.LeyendaCalculoCostoTransporte;
            this.ProductQuotePDF = pq.ProductQuotePDF;
            this.ProductQuoteSmallPDF = pq.ProductQuoteSmallPDF;
            this.Message = pq.Message;
            this.CustomerOrder = pq.CustomerOrder;
            this.DueDateReasonID = pq.DueDateReasonID;
            this.DueDateReason = pq.DueDateReason;
            this.ReasonsForClosureID = pq.ReasonsForClosureID;
            this.ReasonsForClosure = pq.ReasonsForClosure;
            this.ClosureDate = pq.ClosureDate;
            this.ClosureObservations = pq.ClosureObservations;
            this.DateSent = pq.DateSent;

            this.TiempoMedioStockMeses = pq.TiempoMedioStockMeses;
            this.UserId = pq.UserId;

            this.Observations = pq.Observations;
            this.UserObservations = pq.UserObservations;

            this.GV_CostoFinancieroMensual = pq.GV_CostoFinancieroMensual;
            this.GV_CostoAlmacenamientoMensual = pq.GV_CostoAlmacenamientoMensual;
            this.GV_FactorCostoAlmacenamientoMensual = pq.GV_FactorCostoAlmacenamientoMensual;
            this.GV_DiasStockPromedioDistLocal = pq.GV_DiasStockPromedioDistLocal;

            this.FreightType = pq.FreightType;

            this.PrecioVentaRofex = pq.PrecioVentaRofex;
            this.PrecioVentaRofex_PorEntrega = pq.PrecioVentaRofex * pq.MinimumQuantityDelivery;
            this.PrecioVentaRofex_PorOC = pq.PrecioVentaRofex * pq.QuantityOpenPurchaseOrder;

            this.MargenNetoUSDRofex = pq.MargenNetoUSDRofex;
            this.MargenNetoUSDRofex_PorEntrega = pq.MargenNetoUSDRofex * pq.MinimumQuantityDelivery;
            this.MargenNetoUSDRofex_PorOC = pq.MargenNetoUSDRofex * pq.QuantityOpenPurchaseOrder;

            this.MargenNetoPORCRofex = Helper.RoundDecimal(pq.MargenNetoPORCRofex, 2);
            this.MargenNetoPORCRofex_PorEntrega = Helper.RoundDecimal(((pq.MargenNetoUSDRofex * pq.MinimumQuantityDelivery) / this.PrecioVentaRofex_PorEntrega * 100), 2);
            this.MargenNetoPORCRofex_PorOC = Helper.RoundDecimal(((pq.MargenNetoUSDRofex * pq.QuantityOpenPurchaseOrder) / this.PrecioVentaRofex_PorOC * 100), 2);

            this.Flete_PorITEMUSD = pq.Flete_PorITEM / pq.TypeChange;

            this.TiempoMedioStockDias = pq.TiempoMedioStockMeses * 30;
            this.MaximoTiempoStockDias = pq.MaximumMonthsStock * 30;
            this.PaymentDeadlineDays = pq.PaymentDeadlineMonths * 30;
            this.WorkingCapital = this.PaymentDeadlineDays + this.TiempoMedioStockDias + pq.ProductLeadTime - pq.ProductProviderPaymentDeadline + pq.CustomerDelayAverageDays;
            this.AlmacenamientoARS = pq.GV_CostoAlmacenamientoMensual * this.TiempoMedioStockDias / 30;
            this.AlmacenamientoUSD = (pq.GV_CostoAlmacenamientoMensual * this.TiempoMedioStockDias / 30) / pq.TypeChange;
            this.AlmacenamientoUSDKg = ((pq.GV_CostoAlmacenamientoMensual * this.TiempoMedioStockDias / 30) / pq.TypeChange) / 1000;
            this.Inout_USD = pq.Inout_PorITEM / pq.TypeChange;
            this.Inout_USDKg = pq.Inout_PorITEM / pq.TypeChange / 1000;

            this.CostoFinancieroMensual_PorITEM_Formula = pq.CostoFinancieroMensual_PorITEM_Formula;
            this.WorkingCapital_Formula = pq.WorkingCapital_Formula;
            this.TiempoMedioStockDias_Formula = pq.TiempoMedioStockDias_Formula;

            //ROI
            this.MargenBrutoPORCRofex = this.MargenNetoPORCRofex + this.CostoFinancieroMensual_PorITEM;
            if (this.WorkingCapital != 0)
                this.WorkingCapitalRotation = 360 / this.WorkingCapital;
            else
                this.WorkingCapitalRotation = 0;
            this.WorkingCapitalROI = this.MargenBrutoPORCRofex * this.WorkingCapitalRotation;

            this.GVD_CostoAlmacenamientoMensual = pq.GVD_CostoAlmacenamientoMensual;
            this.GVD_CostoInOut = pq.GVD_CostoInOut;
            this.GVD_CostoFinancieroMensual = pq.GVD_CostoFinancieroMensual;
            this.GVD_ImpuestoDebitoCredito = pq.GVD_ImpuestoDebitoCredito;
            this.GVD_GastosFijos = pq.GVD_GastosFijos;
            this.GVD_IIBBAlicuota = pq.GVD_IIBBAlicuota;
            this.GVD_TipoCambio = pq.GVD_TipoCambio;
            this.GVD_FactorCostoAlmacenamientoMensual = pq.GVD_FactorCostoAlmacenamientoMensual;
            this.GVD_DiasStockPromedioDistLocal = pq.GVD_DiasStockPromedioDistLocal;

            this.CostoFleteInput = pq.CostoFleteInput;
            this.PrecioInput = pq.PrecioInput;
            this.MargenInput = pq.MargenInput;
            this.MargenUSDInput = pq.MargenUSDInput;
            this.SellerCommissionInput = pq.SellerCommissionInput;
            this.ProductValidityOfPriceInput = pq.ProductValidityOfPriceInput;
            this.IsSellerUser = pq.IsSellerUser;

            this.FactorRofexInput = pq.FactorRofexInput;

            this.HasWayOfException = pq.HasWayOfException;
            this.WayOfExceptionValue = pq.WayOfExceptionValue;

            this.IsSaleModalityFindParam = pq.IsSaleModalityFindParam;
            this.IsGeographicAreaFindParam = pq.IsGeographicAreaFindParam;
            this.IsPaymentDeadlineFindParam = pq.IsPaymentDeadlineFindParam;
            this.IsQuantityOpenPurchaseOrderFindParam = pq.IsQuantityOpenPurchaseOrderFindParam;
            this.IsDeliveryAmountFindParam = pq.IsDeliveryAmountFindParam;
            this.IsMaximumMonthsStockFindParam = pq.IsMaximumMonthsStockFindParam;
            this.IsExchangeTypeFindParam = pq.IsExchangeTypeFindParam;
            this.IsMinimumQuantityDeliveryFindParam = pq.IsMinimumQuantityDeliveryFindParam;

            this.ExceptionApplyType = pq.ExceptionApplyType;

            this.UserFullName = pq.UserFullName;
            this.UserInitials = pq.User.Initials;
            this.ProductQuotePDFFooter = pq.ProductQuotePDFFooter;
            this.ProductQuoteSmallPDFFooter = pq.ProductQuoteSmallPDFFooter;
            this.ExpressCalc = pq.ExpressCalc;

            this.ProductQuoteStatus = pq.ProductQuoteStatus;
        }
    }
}