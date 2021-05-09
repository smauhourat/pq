using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Library;
using System.ComponentModel;
using ProductQuoteApp.Logging;

namespace ProductQuoteApp.Services
{
    /// <summary>
    /// Ver si tiene Via de Excepcion
    /// </summary>
    public class ProductQuoteService : IProductQuoteService
    {
        private enum EnumExchangeType
        {
            ConVariacionTipoCambio = 1,
            SinVariacionTipoCambio = 2
        }

        private ILogger log = null;
        private IProductQuoteRepository productQuoteRepository = null;
        private IProductRepository productRepository = null;
        private IWorkflowMessageService workflowMessageService = null;
        private IPdfService pdfService = null;
        private ISaleModalityProductRepository saleModalityProductRepository = null;
        private ISaleModalityCreditRatingRepository saleModalityCreditRatingRepository = null;
        private IGlobalVariableRepository globalVariableRepository = null;
        private ICustomerOrderRepository customerOrderRepository = null;
        private ICustomerRepository customerRepository = null;
        private ITransportTypeRepository transportTypeRepository = null;
        private IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepository = null;
        private IPackagingRepository packagingRepository = null;
        private IRofexRepository rofexRepository = null;
        private IPaymentDeadlineRepository paymentDeadlineRepository = null;
        private IShipmentTrackingRepository shipmentTrackingRepository = null;
        private ICustomerOrderService customerOrderService = null;
        private ITransportServices transportServices = null;
        private IMarginServices marginServices = null;
        private IWayOfExceptionServices wayOfExceptionServices = null;
        private ISalesChannelUserRepository salesChannelUserRepository = null;

        public ProductQuoteService(ILogger logger, IProductQuoteRepository productQuoteRepo, IProductRepository productRepo, IWorkflowMessageService workflowMessageServ, IPdfService pdfServ, ISaleModalityProductRepository saleModalityProductRepo, ISaleModalityCreditRatingRepository saleModalityCreditRatingRepo, IGlobalVariableRepository globalVariableRepo, ICustomerOrderRepository customerOrderRepo, ICustomerRepository customerRepo, ITransportTypeRepository transportTypeRepo, IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepo, IPackagingRepository packagingRepo, IRofexRepository rofexRepo, IPaymentDeadlineRepository paymentDeadlineRepo, IShipmentTrackingRepository shipmentTrackingRepo, ICustomerOrderService customerOrderServ, ITransportServices transportServ, IMarginServices marginServ, IWayOfExceptionServices wayOfExceptionServ, ISalesChannelUserRepository salesChannelUserRepo)
        {
            log = logger;
            productQuoteRepository = productQuoteRepo;
            productRepository = productRepo;
            workflowMessageService = workflowMessageServ;
            pdfService = pdfServ;
            saleModalityProductRepository = saleModalityProductRepo;
            saleModalityCreditRatingRepository = saleModalityCreditRatingRepo;
            globalVariableRepository = globalVariableRepo;
            customerOrderRepository = customerOrderRepo;
            customerRepository = customerRepo;
            transportTypeRepository = transportTypeRepo;
            geographicAreaTransportTypeRepository = geographicAreaTransportTypeRepo;
            packagingRepository = packagingRepo;
            rofexRepository = rofexRepo;
            paymentDeadlineRepository = paymentDeadlineRepo;
            shipmentTrackingRepository = shipmentTrackingRepo;
            customerOrderService = customerOrderServ;
            transportServices = transportServ;
            marginServices = marginServ;
            wayOfExceptionServices = wayOfExceptionServ;
            salesChannelUserRepository = salesChannelUserRepo;
        }

        private decimal GetProductCostByProductAndSaleModality(ProductQuote productQuoteToAdd)
        {
            SaleModalityProduct res = saleModalityProductRepository.FindBySaleModalityAndProduct(productQuoteToAdd.SaleModalityID, productQuoteToAdd.ProductID);
            return res.ProductCost;
        }

        public Rofex GetRofex(int paymentDeadlineID)
        {
            PaymentDeadline paymentDeadline = paymentDeadlineRepository.FindPaymentDeadlineByID(paymentDeadlineID);
            int diasPlazo = paymentDeadline.Days;

            if (diasPlazo == 0)
                return null;

            List<Rofex> rofexs = rofexRepository.Rofexs();

            //sino sigo buscando el minimo Rofex ("si eligió 60 días FF, elegimos el TC según ROFEX a 90 días")
            foreach (Rofex item in rofexs)
            {
                if (item.Days > diasPlazo)
                {
                    return item;
                }
            }

            //si no encontro ninguno se queda con el mayor
            return rofexs.LastOrDefault();
        }

        public Rofex GetRofexNuevo(int paymentDeadlineID, int delayAverageDays)
        {
            PaymentDeadline paymentDeadline = paymentDeadlineRepository.FindPaymentDeadlineByID(paymentDeadlineID);
            int diasPlazo = paymentDeadline.Days + delayAverageDays;

            if (diasPlazo == 0)
                return null;

            List<Rofex> rofexs = rofexRepository.Rofexs();

            //sino sigo buscando el minimo Rofex ("si eligió 60 días FF, elegimos el TC según ROFEX a 90 días")
            foreach (Rofex item in rofexs)
            {
                if (item.Days >= diasPlazo)
                {
                    return item;
                }
            }

            //si no encontro ninguno se queda con el mayor
            return rofexs.LastOrDefault();
        }

        private void CalculateLocal(ProductQuote productQuote, decimal factorExcepcion, decimal margenExcepcion, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, decimal margenNetoPorcentual_PorTON, decimal margenMinUsd_PorTON, string margenNetoEntidadOrigen)
        {

            decimal var_IIBBAlicuota = product.IIBBTreatment.Percentage; // globalVariable.IIBBAlicuota;

            //InOuStorage
            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //calculo almacenamiento
            decimal mesesStockMaximo = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;
            decimal mesesStockPromedioDistLocal = (decimal)globalVariable.DiasStockPromedioDistLocal / 30;
            decimal tiempoMedioStockMeses = mesesStockMaximo > mesesStockPromedioDistLocal ? mesesStockMaximo : mesesStockPromedioDistLocal;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;
            decimal denominadorPrecioVenta_PorTON = costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100);
            denominadorPrecioVenta_PorTON = (margenExcepcion != 0 ? denominadorPrecioVenta_PorTON + margenExcepcion : denominadorPrecioVenta_PorTON + margenNetoPorcentual_PorTON);
            denominadorPrecioVenta_PorTON = denominadorPrecioVenta_PorTON + (productQuote.ExchangeInsurance / 100);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVenta = numeradorPrecioVenta_PorTON / (1 - denominadorPrecioVenta_PorTON);
            precioVenta = precioVenta * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN NETO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal numeradorPrecioVentaMargenUSD_PorTON = margenMinUsd_PorTON + (productCost * productQuote.MinimumQuantityDelivery) + (flete_PorTON * productQuote.MinimumQuantityDelivery) + (almacenamientoCosto_PorTON * productQuote.MinimumQuantityDelivery) + (inout_PorTON * productQuote.MinimumQuantityDelivery) + (merma_PorTON * productQuote.MinimumQuantityDelivery);
            decimal denominadorPrecioVentaMargenUSD_PorTON = (1 - ((productQuote.ExchangeInsurance / 100) + costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100)));
            

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVentaMargenUsd = numeradorPrecioVentaMargenUSD_PorTON / denominadorPrecioVentaMargenUSD_PorTON / productQuote.MinimumQuantityDelivery;
            precioVentaMargenUsd = precioVentaMargenUsd * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool calculoPorMargenMinUsd_PorTON = false;

            //Si viene el margen de excepcion, no se toma en cuenta el Margen por USD
            if (margenExcepcion != 0)
                precioVentaMargenUsd = 0;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //SELECCION MEJOR PRECIO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (precioVentaMargenUsd > precioVenta)
            {
                precioVenta = precioVentaMargenUsd;
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor USD]";
                calculoPorMargenMinUsd_PorTON = true;
            }
            else
            {
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor %]";
                calculoPorMargenMinUsd_PorTON = false;
            }

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            if (calculoPorMargenMinUsd_PorTON)
            {
                margenNetoPorcentual_PorTON = (precioVenta - costoTotalCliente) / precioVenta * 100;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Ingresaron a mano un margen en USD por Kilo (ETAPA 3)
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (productQuote.MargenUSDInput > 0)
            if (productQuote.MargenUSDInput != null)
            {
                precioVenta = ((costoTotalCliente * productQuote.QuantityOpenPurchaseOrder) + (decimal)productQuote.MargenUSDInput) / productQuote.QuantityOpenPurchaseOrder;
                productQuote.PriceOriginal = precioVenta;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = precioVenta * factorRofex;
            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "MAYOR([Máximo Tiempo en Stock (Días)] / [Factor Tiempo en Stock]; [Días Stock Promedio Dist. Local])";

        }

        private void CalculateLocalReverso(ProductQuote productQuote, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = product.IIBBTreatment.Percentage; // globalVariable.IIBBAlicuota;

            //InOuStorage
            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //calculo almacenamiento
            decimal mesesStockMaximo = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;
            decimal mesesStockPromedioDistLocal = (decimal)globalVariable.DiasStockPromedioDistLocal / 30;
            decimal tiempoMedioStockMeses = mesesStockMaximo > mesesStockPromedioDistLocal ? mesesStockMaximo : mesesStockPromedioDistLocal;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //REVERSO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = (decimal)productQuote.PrecioInput;
            decimal precioObjetivo = (decimal)productQuote.PrecioInput / factorRofex;
            decimal margenNetoPorcentual_PorTON = 1 - (numeradorPrecioVenta_PorTON / (decimal)productQuote.PrecioInput) - (costoFinancieroMensual_PorItem + (productQuote.ExchangeInsurance / 100) + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100));


            decimal precioVenta = precioObjetivo;

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            //productQuote.ValorRofex = valorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "MAYOR([Máximo Tiempo en Stock (Días)] / [Factor Tiempo en Stock]; [Días Stock Promedio Dist. Local])";

        }

        private void CalculateLocalProgramada(ProductQuote productQuote, decimal factorExcepcion, decimal margenExcepcion, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, decimal margenNetoPorcentual_PorTON, decimal margenMinUsd_PorTON, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = product.IIBBTreatment.Percentage; // globalVariable.IIBBAlicuota;


            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;
            decimal denominadorPrecioVenta_PorTON = costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100);
            denominadorPrecioVenta_PorTON = (margenExcepcion != 0 ? denominadorPrecioVenta_PorTON + margenExcepcion : denominadorPrecioVenta_PorTON + margenNetoPorcentual_PorTON);
            denominadorPrecioVenta_PorTON = denominadorPrecioVenta_PorTON + (productQuote.ExchangeInsurance / 100);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVenta = numeradorPrecioVenta_PorTON / (1 - denominadorPrecioVenta_PorTON);
            precioVenta = precioVenta * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN NETO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal numeradorPrecioVentaMargenUSD_PorTON = margenMinUsd_PorTON + (productCost * productQuote.MinimumQuantityDelivery) + (flete_PorTON * productQuote.MinimumQuantityDelivery) + (almacenamientoCosto_PorTON * productQuote.MinimumQuantityDelivery) + (inout_PorTON * productQuote.MinimumQuantityDelivery) + (merma_PorTON * productQuote.MinimumQuantityDelivery);
            decimal denominadorPrecioVentaMargenUSD_PorTON = (1 - ((productQuote.ExchangeInsurance / 100) + costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100)));


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVentaMargenUsd = numeradorPrecioVentaMargenUSD_PorTON / denominadorPrecioVentaMargenUSD_PorTON / productQuote.MinimumQuantityDelivery;
            precioVentaMargenUsd = precioVentaMargenUsd * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool calculoPorMargenMinUsd_PorTON = false;

            //Si viene el margen de excepcion, no se toma en cuenta el Margen por USD
            if (margenExcepcion != 0)
                precioVentaMargenUsd = 0;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //SELECCION MEJOR PRECIO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (precioVentaMargenUsd > precioVenta)
            {
                precioVenta = precioVentaMargenUsd;
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor USD]";
                calculoPorMargenMinUsd_PorTON = true;
            }
            else
            {
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor %]";
                calculoPorMargenMinUsd_PorTON = false;
            }

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            if (calculoPorMargenMinUsd_PorTON)
            {
                margenNetoPorcentual_PorTON = (precioVenta - costoTotalCliente) / precioVenta * 100;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Ingresaron a mano un margen en USD por Kilo (ETAPA 3)
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (productQuote.MargenUSDInput > 0)
            if (productQuote.MargenUSDInput != null)
            {
                precioVenta = ((costoTotalCliente * productQuote.QuantityOpenPurchaseOrder) + (decimal)productQuote.MargenUSDInput) / productQuote.QuantityOpenPurchaseOrder;
                productQuote.PriceOriginal = precioVenta;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = precioVenta * factorRofex;
            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "[Máximo Tiempo en Stock (Meses)] / [Factor Tiempo en Stock]";

        }

        private void CalculateLocalProgramadaReverso(ProductQuote productQuote, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = product.IIBBTreatment.Percentage; // globalVariable.IIBBAlicuota;

            //InOuStorage
            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //REVERSO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = (decimal)productQuote.PrecioInput;
            decimal precioObjetivo = (decimal)productQuote.PrecioInput / factorRofex;
            decimal margenNetoPorcentual_PorTON = 1 - (numeradorPrecioVenta_PorTON / (decimal)productQuote.PrecioInput) - (costoFinancieroMensual_PorItem + (productQuote.ExchangeInsurance / 100) + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100));


            decimal precioVenta = precioObjetivo;

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            //productQuote.ValorRofex = valorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "[Máximo Tiempo en Stock (Meses)] / [Factor Tiempo en Stock]";

        }

        private void CalculateIndentSL(ProductQuote productQuote, decimal factorExcepcion, decimal margenExcepcion, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, decimal margenNetoPorcentual_PorTON, decimal margenMinUsd_PorTON, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = 0;

            //InOuStorage
            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;
            decimal denominadorPrecioVenta_PorTON = costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100);
            denominadorPrecioVenta_PorTON = (margenExcepcion != 0 ? denominadorPrecioVenta_PorTON + margenExcepcion : denominadorPrecioVenta_PorTON + margenNetoPorcentual_PorTON);
            denominadorPrecioVenta_PorTON = denominadorPrecioVenta_PorTON + (productQuote.ExchangeInsurance / 100);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVenta = numeradorPrecioVenta_PorTON / (1 - denominadorPrecioVenta_PorTON);
            precioVenta = precioVenta * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN NETO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal numeradorPrecioVentaMargenUSD_PorTON = margenMinUsd_PorTON + (productCost * productQuote.MinimumQuantityDelivery) + (flete_PorTON * productQuote.MinimumQuantityDelivery) + (almacenamientoCosto_PorTON * productQuote.MinimumQuantityDelivery) + (inout_PorTON * productQuote.MinimumQuantityDelivery) + (merma_PorTON * productQuote.MinimumQuantityDelivery);
            decimal denominadorPrecioVentaMargenUSD_PorTON = (1 - ((productQuote.ExchangeInsurance / 100) + costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100)));


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVentaMargenUsd = numeradorPrecioVentaMargenUSD_PorTON / denominadorPrecioVentaMargenUSD_PorTON / productQuote.MinimumQuantityDelivery;
            precioVentaMargenUsd = precioVentaMargenUsd * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool calculoPorMargenMinUsd_PorTON = false;

            //Si viene el margen de excepcion, no se toma en cuenta el Margen por USD
            if (margenExcepcion != 0)
                precioVentaMargenUsd = 0;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //SELECCION MEJOR PRECIO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (precioVentaMargenUsd > precioVenta)
            {
                precioVenta = precioVentaMargenUsd;
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor USD]";
                calculoPorMargenMinUsd_PorTON = true;
            }
            else
            {
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor %]";
                calculoPorMargenMinUsd_PorTON = false;
            }

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            if (calculoPorMargenMinUsd_PorTON)
            {
                margenNetoPorcentual_PorTON = (precioVenta - costoTotalCliente) / precioVenta * 100;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Ingresaron a mano un margen en USD por Kilo (ETAPA 3)
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (productQuote.MargenUSDInput > 0)
            if (productQuote.MargenUSDInput != null)
            {
                precioVenta = ((costoTotalCliente * productQuote.QuantityOpenPurchaseOrder) + (decimal)productQuote.MargenUSDInput) / productQuote.QuantityOpenPurchaseOrder;
                productQuote.PriceOriginal = precioVenta;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = precioVenta * factorRofex;
            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            //productQuote.ValorRofex = valorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "[Máximo Tiempo en Stock (Meses)] / [Factor Tiempo en Stock]";

        }

        private void CalculateIndentSLReverso(ProductQuote productQuote, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = 0;

            //InOuStorage
            if (product.BuyAndSellDirect || !product.InOutStorage)
            {
                globalVariable.CostoAlmacenamientoMensual = 0;
                globalVariable.CostoInOut = 0;

            }

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            //InOuStorage
            tiempoMedioStockMeses = (product.BuyAndSellDirect || !product.InOutStorage ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = globalVariable.CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = globalVariable.CostoAlmacenamientoMensual;

            decimal inout_PorTON = globalVariable.CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = ((productQuote.PaymentDeadlineMonths + tiempoMedioStockMeses - ((decimal)product.ProviderPaymentDeadline / 30) + ((decimal)product.LeadTime) / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //REVERSO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = (decimal)productQuote.PrecioInput;
            decimal precioObjetivo = (decimal)productQuote.PrecioInput / factorRofex;
            decimal margenNetoPorcentual_PorTON = 1 - (numeradorPrecioVenta_PorTON / (decimal)productQuote.PrecioInput) - (costoFinancieroMensual_PorItem + (productQuote.ExchangeInsurance / 100) + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100));


            decimal precioVenta = precioObjetivo;

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = globalVariable.CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            //productQuote.ValorRofex = valorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) + productQuote.ProductLeadTime - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Lead Time (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] + [Lead Time (Días desde Factura)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "[Máximo Tiempo en Stock (Meses)] / [Factor Tiempo en Stock]";

        }

        private void CalculateIndent(ProductQuote productQuote, decimal factorExcepcion, decimal margenExcepcion, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, decimal margenNetoPorcentual_PorTON, decimal margenMinUsd_PorTON, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = 0;
            decimal var_CostoAlmacenamientoMensual = 0;
            decimal var_CostoInOut = 0;
            product.Waste = 0; // Siempre 0

            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            tiempoMedioStockMeses = (product.BuyAndSellDirect ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = var_CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = var_CostoAlmacenamientoMensual;

            decimal inout_PorTON = var_CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = (productQuote.PaymentDeadlineMonths - ((decimal)product.ProviderPaymentDeadline / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;
            decimal denominadorPrecioVenta_PorTON = costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100);
            denominadorPrecioVenta_PorTON = (margenExcepcion != 0 ? denominadorPrecioVenta_PorTON + margenExcepcion : denominadorPrecioVenta_PorTON + margenNetoPorcentual_PorTON);
            denominadorPrecioVenta_PorTON = denominadorPrecioVenta_PorTON + (productQuote.ExchangeInsurance / 100);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVenta = numeradorPrecioVenta_PorTON / (1 - denominadorPrecioVenta_PorTON);
            precioVenta = precioVenta * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN NETO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal numeradorPrecioVentaMargenUSD_PorTON = margenMinUsd_PorTON + (productCost * productQuote.MinimumQuantityDelivery) + (flete_PorTON * productQuote.MinimumQuantityDelivery) + (almacenamientoCosto_PorTON * productQuote.MinimumQuantityDelivery) + (inout_PorTON * productQuote.MinimumQuantityDelivery) + (merma_PorTON * productQuote.MinimumQuantityDelivery);
            decimal denominadorPrecioVentaMargenUSD_PorTON = (1 - ((productQuote.ExchangeInsurance / 100) + costoFinancieroMensual_PorItem + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100)));


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal precioVentaMargenUsd = numeradorPrecioVentaMargenUSD_PorTON / denominadorPrecioVentaMargenUSD_PorTON / productQuote.MinimumQuantityDelivery;
            precioVentaMargenUsd = precioVentaMargenUsd * factorExcepcion;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool calculoPorMargenMinUsd_PorTON = false;

            //Si viene el margen de excepcion, no se toma en cuenta el Margen por USD
            if (margenExcepcion != 0)
                precioVentaMargenUsd = 0;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //SELECCION MEJOR PRECIO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (precioVentaMargenUsd > precioVenta)
            {
                precioVenta = precioVentaMargenUsd;
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor USD]";
                calculoPorMargenMinUsd_PorTON = true;
            }
            else
            {
                margenNetoEntidadOrigen = margenNetoEntidadOrigen + " [Mayor %]";
                calculoPorMargenMinUsd_PorTON = false;
            }

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            if (calculoPorMargenMinUsd_PorTON)
            {
                margenNetoPorcentual_PorTON = (precioVenta - costoTotalCliente) / precioVenta * 100;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Ingresaron a mano un margen en USD por Kilo (ETAPA 3)
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (productQuote.MargenUSDInput > 0)
            if (productQuote.MargenUSDInput != null)
            {
                precioVenta = ((costoTotalCliente * productQuote.QuantityOpenPurchaseOrder) + (decimal)productQuote.MargenUSDInput) / productQuote.QuantityOpenPurchaseOrder;
                productQuote.PriceOriginal = precioVenta;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = var_CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = precioVenta * factorRofex;
            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            //productQuote.ValorRofex = valorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "SIEMPRE CERO";

        }

        private void CalculateIndentReverso(ProductQuote productQuote, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, string margenNetoEntidadOrigen)
        {
            decimal var_IIBBAlicuota = 0;
            decimal var_CostoAlmacenamientoMensual = 0;
            decimal var_CostoInOut = 0;
            product.Waste = 0; // Siempre 0


            Rofex rofex2 = GetRofexNuevo(productQuote.PaymentDeadlineID, customer.DelayAverageDays);
            if (((EnumExchangeType)productQuote.ExchangeTypeID == EnumExchangeType.SinVariacionTipoCambio) && (rofex2 != null))
            {
                productQuote.ExchangeInsurance = (1 - globalVariable.TipoCambio / rofex2.DollarQuotation) * 100;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //PRECIO VENTA MARGEN PORCENTUAL
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //flete/tc/cantminentrega
            decimal flete_PorTON = transportCost / globalVariable.TipoCambio / productQuote.QuantityOpenPurchaseOrder;

            //UNICO CAMBIO DE LA MODALIDAD LOCAL
            //calculo almacenamiento
            decimal tiempoMedioStockMeses = (decimal)productQuote.MaximumMonthsStock / globalVariable.FactorCostoAlmacenamientoMensual;

            tiempoMedioStockMeses = (product.BuyAndSellDirect ? 0 : tiempoMedioStockMeses);

            decimal almacenamientoCosto_PorTON = var_CostoAlmacenamientoMensual / 1000 / globalVariable.TipoCambio * tiempoMedioStockMeses;
            decimal almacenamientoCostoPorItem = var_CostoAlmacenamientoMensual;

            decimal inout_PorTON = var_CostoInOut / globalVariable.TipoCambio / 1000;
            decimal merma_PorTON = ((decimal)product.Waste / 100) * productCost;


            decimal costoFinancieroMensual_PorItem = (productQuote.PaymentDeadlineMonths - ((decimal)product.ProviderPaymentDeadline / 30)) * (globalVariable.CostoFinancieroMensual / 100);


            decimal numeradorPrecioVenta_PorTON = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //REVERSO
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal factorRofex = 1;

            productQuote.PrecioVentaRofex = (decimal)productQuote.PrecioInput;
            decimal precioObjetivo = (decimal)productQuote.PrecioInput / factorRofex;
            decimal margenNetoPorcentual_PorTON = 1 - (numeradorPrecioVenta_PorTON / (decimal)productQuote.PrecioInput) - (costoFinancieroMensual_PorItem + (productQuote.ExchangeInsurance / 100) + (globalVariable.GastosFijos / 100) + (globalVariable.ImpuestoDebitoCredito / 100) + (var_IIBBAlicuota / 100) + (customer.SellerCommission / 100));


            decimal precioVenta = precioObjetivo;

            //v0.7 Reservamos el Precio Calculado Original
            productQuote.PriceOriginal = precioVenta;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //DEDUCCIONES
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal costoFinancieroMensual_PorTON = costoFinancieroMensual_PorItem * precioVenta;
            decimal exchangeInsurance_PorTON = (productQuote.ExchangeInsurance / 100) * precioVenta;
            decimal gastosFijos_PorTON = (globalVariable.GastosFijos / 100) * precioVenta;
            decimal impuestoDC_PorTON = (globalVariable.ImpuestoDebitoCredito / 100) * precioVenta;
            decimal iibbAlicuota_PorTON = (var_IIBBAlicuota / 100) * precioVenta;
            decimal comisiones_PorTON = (customer.SellerCommission / 100) * precioVenta;

            decimal costoTotalCliente = productCost + flete_PorTON + almacenamientoCosto_PorTON + inout_PorTON + merma_PorTON + costoFinancieroMensual_PorTON + gastosFijos_PorTON + impuestoDC_PorTON + iibbAlicuota_PorTON + comisiones_PorTON;
            costoTotalCliente = costoTotalCliente + exchangeInsurance_PorTON;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //RESULTADOS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            productQuote.Price = precioVenta;
            productQuote.CustomerTotalCost = costoTotalCliente;

            //Calculos
            productQuote.CostoProducto_PorTON = productCost;
            productQuote.TypeChange = globalVariable.TipoCambio;
            productQuote.Flete_PorITEM = transportCost;
            productQuote.Flete_PorTON = flete_PorTON;
            productQuote.AlmacenamientoCosto_PorITEM = almacenamientoCostoPorItem;
            productQuote.AlmacenamientoCosto_PorTON = almacenamientoCosto_PorTON;
            productQuote.Inout_PorITEM = var_CostoInOut;
            productQuote.Inout_PorTON = inout_PorTON;
            productQuote.Merma_PorITEM = (decimal)product.Waste;
            productQuote.Merma_PorTON = merma_PorTON;
            productQuote.CostoFinancieroMensual_PorITEM = costoFinancieroMensual_PorItem * 100;
            productQuote.CostoFinancieroMensual_PorTON = costoFinancieroMensual_PorTON;

            productQuote.ExchangeInsurance_PorTON = exchangeInsurance_PorTON;

            productQuote.GastosFijos_PorITEM = globalVariable.GastosFijos;
            productQuote.GastosFijos_PorTON = gastosFijos_PorTON;
            productQuote.ImpuestoDC_PorITEM = globalVariable.ImpuestoDebitoCredito;
            productQuote.ImpuestoDC_PorTON = impuestoDC_PorTON;
            productQuote.IIBBAlicuota_PorITEM = var_IIBBAlicuota;
            productQuote.IIBBAlicuota_PorTON = iibbAlicuota_PorTON;
            productQuote.Comisiones_PorITEM = customer.SellerCommission;
            productQuote.Comisiones_PorTON = comisiones_PorTON;

            productQuote.MargenNetoUSD_PorTON = (precioVenta - costoTotalCliente);
            productQuote.MargenNetoPorc_PorTON = margenNetoPorcentual_PorTON * 100;
            productQuote.MargenNetoEntidadOrigen = margenNetoEntidadOrigen;


            productQuote.MargenNetoOriginalUSD_PorTON = productQuote.PriceOriginal - costoTotalCliente;
            productQuote.MargenNetoOriginalPorc_PorTON = Helper.RoundDecimal((productQuote.PriceOriginal - costoTotalCliente) / productQuote.PriceOriginal * 100, 2);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            productQuote.Price = productQuote.PrecioVentaRofex;
            productQuote.FactorRofex = factorRofex;
            productQuote.ValorRofex = rofex2 != null ? rofex2.DollarQuotation : globalVariable.TipoCambio;
            productQuote.MargenNetoUSDRofex = productQuote.PrecioVentaRofex - costoTotalCliente;
            productQuote.MargenNetoPORCRofex = Helper.RoundDecimal((productQuote.PrecioVentaRofex - costoTotalCliente) / productQuote.PrecioVentaRofex * 100, 2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            productQuote.TiempoMedioStockMeses = tiempoMedioStockMeses;
            productQuote.WorkingCapital = (productQuote.PaymentDeadlineMonths * 30) + (productQuote.TiempoMedioStockMeses * 30) - productQuote.ProductProviderPaymentDeadline + productQuote.CustomerDelayAverageDays;

            productQuote.CostoFinancieroMensual_PorITEM_Formula = "( [Condición de Pago (Dias)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]) * [Costo Financiero (TEM %)]";
            productQuote.WorkingCapital_Formula = "[Condición de Pago (Dias)] + [Tiempo Medio en Stock (Días)] - [Plazo Pago Proveedor (Días desde Factura)] + [Promedio de Atraso en el Pago (Días)]";
            productQuote.TiempoMedioStockDias_Formula = "SIEMPRE CERO";

        }

        public bool isValidProductQuoteInput(ProductQuote productQuote)
        {
            if (productQuote.ProductID <= 0)
            {
                productQuote.Message = "Debe ingresar un Producto";
                return false;
            }

            if ((productQuote.QuantityOpenPurchaseOrder <= 0) && (EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.Local)
            {
                productQuote.Message = "Debe ingresar Cantidad Total  Orden de Compra (Kg)";
                return false;
            }

            if (productQuote.MinimumQuantityDelivery <= 0)
            {
                productQuote.Message = "Debe seleccionar la Cantidad de Entregas.";
                return false;
            }

            //La cantidad minima no puede ser mayor a la cantidad total en todos los casos
            if (productQuote.MinimumQuantityDelivery > productQuote.QuantityOpenPurchaseOrder)
            {
                productQuote.Message = "La Cantidad Minima por Entrega (Kg) no puede ser mayor a la Cantidad Total  Orden de Compra (Kg)";
                return false;
            }

            if (((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.IndentSL) && (productQuote.MaximumMonthsStock == 0))
            {
                productQuote.Message = "Debe ingresar Maximo Tiempo en Stock (Meses)";
                return false;
            }

            Product product = productRepository.FindProductsByID(productQuote.ProductID);

            //Para la Modalidad "Distribucion con Importacion a Pedido", la Cantidad Total debe ser multiplo de la cantidad FCL del Producto en Toneladas
            if ( ((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.LocalProgramada) && ((productQuote.QuantityOpenPurchaseOrder % (product.FCLKilogram)) > 0) )
            {
                productQuote.Message = "Para la modalidad Distribución con Importación a Pedido, la Cantidad Total  Orden de Compra (Kg) debe ser multiplo de la Capacidad del FCL: " + ((int)(product.FCLKilogram)).ToString() + " (Kg)";
                return false;
            }

            //Para la Modalidad Indent + SL, la Cantidad Total debe ser multiplo de la cantidad FCL del Producto en Toneladas
            if ( ((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.IndentSL) && ((productQuote.QuantityOpenPurchaseOrder % (product.FCLKilogram / 1000)) > 0) )
            {
                productQuote.Message = "Para la modalidad Indent / Indent + Servicio Logistico/Financiero de guarda, la Cantidad Total  Orden de Compra (Kg) debe ser multiplo de la Capacidad del FCL: " + ((int)(product.FCLKilogram)).ToString() + " (Kg)";
                return false;
            }

            //Para la modalidad LOCAL, la Cantidad Total debe ser multiplo de la Posicion en Kg del Producto
            if ( ((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.Local) && ((productQuote.QuantityOpenPurchaseOrder % (product.PositionKilogram / 1000)) > 0) ) 
            {
                productQuote.Message = "La Cantidad Total  Orden de Compra (Kg) debe ser multiplo de Unidad Mínima de Venta del Producto (" + ((int)(product.PositionKilogram)).ToString() + "Kg.)";
                return false;
            }

            //Para la modalidad LOCAL, la Cantidad Total dividido la Cantidad de Entregas debe ser mayor a la Posicion en Kg del Producto
            if ( ((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.Local) && ((productQuote.QuantityOpenPurchaseOrder / productQuote.DeliveryAmount) < product.PositionKilogram) )
            {
                productQuote.Message = "Cada entrega debe ser mayor a la Unidad Mínima de Venta del Producto (" + ((int)(product.PositionKilogram)).ToString() + " Kg.)";
                return false;
            }

            //Para la modalidad INDENT, la Cantidad Minima debe ser multiplo de la cantidad FCL del Producto en Toneladas
            if (((EnumSaleModality)productQuote.SaleModalityID == EnumSaleModality.Indent) && ((productQuote.MinimumQuantityDelivery % (product.FCLKilogram / 1000)) > 0) )
            {
                productQuote.Message = "Para la modalidad Indent, la Cantidad Minima de la OC (Kg) debe ser multiplo de la capacidad del FCL (" + ((int)(product.FCLKilogram)).ToString() + " Kg.)";
                return false;
            }

            if (productQuote.GVD_FactorCostoAlmacenamientoMensual <= 0)
            {
                productQuote.Message = "El Valor del 'Factor Tiempo en Stock' debe ser mayor a cero.";
                return false;
            }

            if (productQuote.GVD_TipoCambio <= 0)
            {
                productQuote.Message = "El Valor del 'Tipo Cambio (ARS/USD)' debe ser mayor a cero.";
                return false;
            }

            return true;
        }

        private GlobalVariable GetGlobalVariable(ProductQuote productQuote)
        {
            productQuote.GV_CostoAlmacenamientoMensual = productQuote.GVD_CostoAlmacenamientoMensual;
            productQuote.GV_CostoFinancieroMensual = productQuote.GVD_CostoFinancieroMensual;
            productQuote.GV_FactorCostoAlmacenamientoMensual = productQuote.GVD_FactorCostoAlmacenamientoMensual;
            productQuote.GV_DiasStockPromedioDistLocal = productQuote.GVD_DiasStockPromedioDistLocal;

            GlobalVariable globalVariable = new GlobalVariable();
            globalVariable.CostoAlmacenamientoMensual = productQuote.GVD_CostoAlmacenamientoMensual;
            globalVariable.CostoInOut = productQuote.GVD_CostoInOut;
            globalVariable.CostoFinancieroMensual = productQuote.GVD_CostoFinancieroMensual;
            globalVariable.ImpuestoDebitoCredito = productQuote.GVD_ImpuestoDebitoCredito;
            globalVariable.GastosFijos = productQuote.GVD_GastosFijos;
            globalVariable.IIBBAlicuota = productQuote.GVD_IIBBAlicuota;
            globalVariable.TipoCambio = productQuote.GVD_TipoCambio;
            globalVariable.FactorCostoAlmacenamientoMensual = productQuote.GVD_FactorCostoAlmacenamientoMensual;
            globalVariable.DiasStockPromedioDistLocal = productQuote.GVD_DiasStockPromedioDistLocal;

            return globalVariable;
        }

        private ProductQuote GetProductQuoteWithWayOfException(ProductQuote productQuote)
        {
            ProductQuote productQuoteException = (ProductQuote)productQuote.Clone();
            WayOfException wayOfException = null;

            wayOfException = wayOfExceptionServices.HasWayOfExceptionProductQuote(productQuoteException);

            if (wayOfException != null)
            {
                productQuoteException.SaleModalityID = wayOfException.SaleModalityID;
                productQuoteException.GeographicAreaID = wayOfException.GeographicAreaID;
                productQuoteException.PaymentDeadlineID = wayOfException.PaymentDeadlineID;
                productQuoteException.PaymentDeadlineMonths = paymentDeadlineRepository.FindPaymentDeadlineByID(wayOfException.PaymentDeadlineID).Months;

                productQuoteException.QuantityOpenPurchaseOrder = wayOfException.QuantityOpenPurchaseOrder;
                productQuoteException.MinimumQuantityDelivery = (int)wayOfException.MinimumQuantityDelivery;
                productQuoteException.DeliveryAmount = wayOfException.DeliveryAmount;
                productQuoteException.MaximumMonthsStock = wayOfException.MaximumMonthsStock;
                productQuoteException.ExchangeTypeID = wayOfException.ExchangeTypeID;

                productQuoteException.PrecioInput = wayOfException.ExceptionPrice;
                productQuoteException.HasWayOfException = true;
                productQuoteException.ExceptionApplyType = wayOfException.ExceptionApplyType;

                productQuote.HasWayOfException = true;
                productQuote.ExceptionApplyType = wayOfException.ExceptionApplyType;

                productQuote.IsSaleModalityFindParam = productQuoteException.IsSaleModalityFindParam;
                productQuote.IsGeographicAreaFindParam = productQuoteException.IsGeographicAreaFindParam;
                productQuote.IsPaymentDeadlineFindParam = productQuoteException.IsPaymentDeadlineFindParam;
                productQuote.IsQuantityOpenPurchaseOrderFindParam = productQuoteException.IsQuantityOpenPurchaseOrderFindParam;
                productQuote.IsDeliveryAmountFindParam = productQuoteException.IsDeliveryAmountFindParam;
                productQuote.IsMaximumMonthsStockFindParam = productQuoteException.IsMaximumMonthsStockFindParam;
                productQuote.IsExchangeTypeFindParam = productQuoteException.IsExchangeTypeFindParam;
                productQuote.IsMinimumQuantityDeliveryFindParam = productQuoteException.IsMinimumQuantityDeliveryFindParam;

                return productQuoteException;
            }

            return null;
        }

        private void CalculateGeneralReverso(ProductQuote productQuote, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, string margenNetoEntidadOrigen)
        {
            switch ((EnumSaleModality)productQuote.SaleModalityID)
            {
                case EnumSaleModality.Local:
                    CalculateLocalReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.LocalProgramada:
                    CalculateLocalProgramadaReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.IndentSL:
                    CalculateIndentSLReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.Indent:
                    CalculateIndentReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
                    break;
            }
        }

        private void CalculateGeneral(ProductQuote productQuote, decimal factorExcepcion, decimal margenExcepcion, Customer customer, decimal productCost, Product product, GlobalVariable globalVariable, decimal transportCost, decimal margenNetoPorcentual_PorTON, decimal margenMinUsd_PorTON, string margenNetoEntidadOrigen)
        {
            switch ((EnumSaleModality)productQuote.SaleModalityID)
            {
                case EnumSaleModality.Local:
                    CalculateLocal(productQuote, factorExcepcion, margenExcepcion, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.LocalProgramada:
                    CalculateLocalProgramada(productQuote, factorExcepcion, margenExcepcion, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.IndentSL:
                    CalculateIndentSL(productQuote, factorExcepcion, margenExcepcion, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                    break;
                case EnumSaleModality.Indent:
                    CalculateIndent(productQuote, factorExcepcion, margenExcepcion, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                    break;
            }
        }

        //Refactorizar
        public void CalculateQuote(ProductQuote productQuote)
        {

            ProductQuote productQuoteExceptionNormal;
            decimal factorExcepcion = 1;
            decimal margenExcepcion = 0;

            //Validamos la Cotizacion
            if (!isValidProductQuoteInput(productQuote))
                return;

            //Obtenemos el Cliente asociado a la Cotizacion
            Customer customer = customerRepository.FindCustomersByID(productQuote.CustomerID);
            if (customer == null)
            {
                productQuote.Message = "Ha ocurrido un error, el Cliente es inexistente";
                return;
            }

            //Si viene la Comision de Cliente como Input, se pisa la del Cliente
            if (productQuote.SellerCommissionInput.HasValue)
            {
                customer.SellerCommission = (decimal)productQuote.SellerCommissionInput;
            }

            //Obtenemos el producto asociado a la Cotizacion
            Product product = productRepository.FindProductsByID(productQuote.ProductID);
            if (product == null)
            {
                productQuote.Message = "Ha ocurrido un error, el Producto es inexistente";
                return;
            }

            //Si viene la Validez del Precio del Producto, se pisa la del Producto
            if (productQuote.ProductValidityOfPriceInput.HasValue)
            {
                productQuote.ProductValidityOfPrice = (DateTime)productQuote.ProductValidityOfPriceInput;
            }
            //Seteamos el Tipo de Carga
            productQuote.FreightType = product.FreightType.Description;

            //Calculamos el Costo del Flete
            decimal transportCost = 0;
            if (productQuote.CostoFleteInput > 0)
                transportCost = (decimal)productQuote.CostoFleteInput;
            else
                transportCost = transportServices.GetFleteFRMT_PorItem(productQuote, product);

            //Calculamos el Costo del Producto
            decimal productCost = GetProductCostByProductAndSaleModality(productQuote);

            //Obtenemos las Variables Globales
            GlobalVariable globalVariable = GetGlobalVariable(productQuote);

            //Asignamos los valores de Margenes al Producto y al Cliente dependiendo de la Modalidad de Venta
            product.MinimumMarginPercentage = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginPercentage;
            product.MinimumMarginUSD = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginUSD;

            customer.MinimumMarginPercentage = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginPercentage;
            customer.MinimumMarginUSD = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginUSD;

            //Calculamos los Margenes
            SaleModalityCreditRating saleModalityCreditRating = saleModalityCreditRatingRepository.FindSaleModalityCreditRatingByID(productQuote.SaleModalityID, productQuote.CustomerID);
            MininumMarginSale minMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);
            decimal margenNetoPorcentual_PorTON = minMarginSale.MinimumMarginPercentage / 100;
            decimal margenMinUsd_PorTON = minMarginSale.MinimumMarginUSD;
            string margenNetoEntidadOrigen = "Margen " + Helper.RoundDecimal(minMarginSale.MinimumMarginPercentage, 2).ToString() + "% (" + minMarginSale.MininumMarginSourcePercentage.Description() + ") - Margen " + Helper.RoundDecimal(minMarginSale.MinimumMarginUSD, 2).ToString() + "USD (" + minMarginSale.MininumMarginSourceUSD.Description() + ")";

            //Buscamos la Via de Excepcion
            ProductQuote productQuoteException = GetProductQuoteWithWayOfException(productQuote);

            if (productQuote.PrecioInput > 0)
            {
                CalculateGeneralReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
            }
            else
            {
                //Si tiene Via de Excepcion, (pero no se ingreso Margen) devuelve en productQuoteException, la cotizacion con la excepcion
                if ((productQuoteException != null) && (productQuote.MargenInput == null))
                {
                    if (productQuoteException.ExceptionApplyType == ExceptionApplyType.ApplyPriceDirect)
                    {
                        productQuote.PrecioInput = productQuoteException.PrecioInput;
                        CalculateGeneralReverso(productQuote, customer, productCost, product, globalVariable, transportCost, margenNetoEntidadOrigen);
                        productQuote.WayOfExceptionValue = (decimal)productQuote.PrecioInput;
                    }
                    else
                    {
                        decimal transportCostException = transportServices.GetFleteFRMT_PorItem(productQuoteException, product);
                        productQuoteExceptionNormal = (ProductQuote)productQuoteException.Clone();


                        //Asignamos los valores de Margenes al Producto y al Cliente dependiendo de la Modalidad de Venta
                        product.MinimumMarginPercentage = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuoteException.SaleModalityID)?.MinimumMarginPercentage;
                        product.MinimumMarginUSD = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuoteException.SaleModalityID)?.MinimumMarginUSD;

                        customer.MinimumMarginPercentage = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuoteException.SaleModalityID)?.MinimumMarginPercentage;
                        customer.MinimumMarginUSD = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuoteException.SaleModalityID)?.MinimumMarginUSD;

                        //Calculamos los Margenes
                        SaleModalityCreditRating saleModalityCreditRating2 = saleModalityCreditRatingRepository.FindSaleModalityCreditRatingByID(productQuoteException.SaleModalityID, productQuoteException.CustomerID);
                        MininumMarginSale minMarginSale2 = marginServices.GetMargenNetoPorcentual(productQuoteException, product, customer, saleModalityCreditRating2);
                        decimal margenNetoPorcentual_PorTON2 = minMarginSale2.MinimumMarginPercentage / 100;
                        decimal margenMinUsd_PorTON2 = minMarginSale2.MinimumMarginUSD;
                        string margenNetoEntidadOrigen2 = "Margen " + Helper.RoundDecimal(minMarginSale2.MinimumMarginPercentage, 2).ToString() + "% (" + minMarginSale2.MininumMarginSourcePercentage.Description() + ") - Margen " + Helper.RoundDecimal(minMarginSale2.MinimumMarginUSD, 2).ToString() + "USD (" + minMarginSale2.MininumMarginSourceUSD.Description() + ")";

                        //Calculamos el Costo del Producto
                        decimal productCost2 = GetProductCostByProductAndSaleModality(productQuoteExceptionNormal);


                        //Calculo con parametros de Excepcion a Precio Cotizador
                        CalculateGeneral(productQuoteExceptionNormal, 1, 0, customer, productCost2, product, globalVariable, transportCostException, margenNetoPorcentual_PorTON2, margenMinUsd_PorTON2, margenNetoEntidadOrigen2);
                        //Calculo con parametros de Excepcion a Precio Excepcion
                        CalculateGeneralReverso(productQuoteException, customer, productCost2, product, globalVariable, transportCostException, margenNetoEntidadOrigen2);


                        factorExcepcion = productQuoteException.ExceptionApplyType == ExceptionApplyType.ApplyProporcionalPrice ? productQuoteException.PriceOriginal / productQuoteExceptionNormal.PriceOriginal : 1;
                        margenExcepcion = productQuoteException.ExceptionApplyType == ExceptionApplyType.ApplyEquivalentMargin ? productQuoteException.MargenNetoOriginalPorc_PorTON / 100 : 0;
                        if (productQuote.ExceptionApplyType == ExceptionApplyType.ApplyProporcionalPrice)
                            productQuote.WayOfExceptionValue = factorExcepcion;
                        if (productQuote.ExceptionApplyType == ExceptionApplyType.ApplyEquivalentMargin)
                            productQuote.WayOfExceptionValue = margenExcepcion*100;
                        if (productQuote.ExceptionApplyType == ExceptionApplyType.ApplyPriceDirect)
                            productQuote.WayOfExceptionValue = 1;


                        product.MinimumMarginPercentage = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginPercentage;
                        product.MinimumMarginUSD = product.SaleModalityProductMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginUSD;

                        customer.MinimumMarginPercentage = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginPercentage;
                        customer.MinimumMarginUSD = customer.SaleModalityCustomerMargins.FirstOrDefault(m => m.SaleModalityID == productQuote.SaleModalityID)?.MinimumMarginUSD;

                        //Calculo utilizando el factor de excepcion
                        CalculateGeneral(productQuote, factorExcepcion, margenExcepcion, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                    }
                }
                else
                {
                    CalculateGeneral(productQuote, 1, 0, customer, productCost, product, globalVariable, transportCost, margenNetoPorcentual_PorTON, margenMinUsd_PorTON, margenNetoEntidadOrigen);
                }
            }


            if (productQuote.Price < 0)
            {
                productQuote.Message = "Parametros no aceptados, consulte con el Administrador";
            }
        }

        public void CreateQuote(string customerUserEmail, ProductQuote productQuote, Boolean sendNotifications, Boolean sendClientNotifications, Boolean sendUserCreatorNotifications)
        {
            try
            {
                //Validamos la Cotizacion
                if (!isValidProductQuoteInput(productQuote))
                    return;

                //Si viene la Validez del Precio del Producto, se pisa la del Producto
                if (productQuote.ProductValidityOfPriceInput.HasValue)
                {
                    productQuote.ProductValidityOfPrice = (DateTime)productQuote.ProductValidityOfPriceInput;
                }

                //se genera el pdf con la cotizacion
                Product product = productRepository.FindProductsByID(productQuote.ProductID);

                //productQuoteToAdd.DateQuote = DateTime.UtcNow;
                productQuote.DateQuote = DateTime.Now;

                //Se guarda la cotizacion
                productQuoteRepository.Create(productQuote);

                //Creamos el PDF
                productQuote.ProductQuotePDF = pdfService.ProductQuoteToPdf(productQuote, product.ProductDocuments, product.SellerCompany.ProductQuotePdfTemplate);

                //Creamos el SmallPDF
                productQuote.ProductQuoteSmallPDF = pdfService.ProductQuoteToSmallPdf(productQuote, product.SellerCompany.ProductQuoterSmallPdfTemplate);

                //Actualizamos la referencias a los Pdfs en la Cotizacion
                productQuoteRepository.UpdatePdf(productQuote);

                //Se modifica el Tracking
                ShipmentTracking shipmentTracking = new ShipmentTracking { ProductQuoteID = productQuote.ProductQuoteID, QuotedEstimatedDate = productQuote.DateQuote, QuotedRealDate = productQuote.DateQuote, QuotedCompleted = true };
                shipmentTrackingRepository.Create(shipmentTracking);

                if (sendNotifications)
                {
                    //se envia la cotizacion a los administradores
                    workflowMessageService.SendAdministratorProductQuote(productQuote);
                }
                if (sendClientNotifications)
                {
                    //se envia la notificacion al cliente
                    workflowMessageService.SendCustomerProductQuote(customerUserEmail, productQuote);
                }
                if (sendUserCreatorNotifications)
                {
                    workflowMessageService.SendUserCreatorProductQuote(productQuote);
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductQuoteServices.Create()");
                throw;
            }

        }

        public void CreateCustomerOrder(string customerUserEmail, ProductQuote productQuote, Boolean createOCAsApproved)
        {
            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.ProductQuoteID = productQuote.ProductQuoteID;
            customerOrder.DateOrder = DateTime.Now;

            customerOrderRepository.Create(customerOrder);

            ShipmentTracking shipmentTracking = new ShipmentTracking { ProductQuoteID = productQuote.ProductQuoteID, CustomerOrderEstimatedDate = customerOrder.DateOrder, CustomerOrderRealDate = customerOrder.DateOrder, CustomerOrderCompleted = true };

            if (createOCAsApproved)
            {
                shipmentTracking.ApprovedCompleted = true;
                shipmentTracking.ApprovedEstimatedDate = customerOrder.DateOrder;
                shipmentTracking.ApprovedRealDate = customerOrder.DateOrder;
            }

            shipmentTrackingRepository.Update(shipmentTracking);

        }

        public async Task DeleteAsync(int productQuoteID)
        {
            pdfService.DeleteProductQuotePdf(productQuoteRepository.FindProductQuotesByID(productQuoteID));
            pdfService.DeleteProductQuoteSmallPdf(productQuoteRepository.FindProductQuotesByID(productQuoteID));

            await customerOrderService.DeleteAsync(productQuoteID);

            await productQuoteRepository.DeleteAsync(productQuoteID);
        }

        /// <summary>
        /// Filtrados por Canales de Ventas del Current User
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="search"></param>
        /// <param name="isQuote"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public async Task<List<ProductQuote>> FindProductQuotesByAdminUserAsync(string currentUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo)
        {
            var productQuotes = await productQuoteRepository.FindProductQuotesByUserIDAsync(null, search, isQuote, dateFrom, dateTo);
            var salesChannelUser = await salesChannelUserRepository.FindSalesChannelsByUserIDAsync(currentUserId);
            var result = productQuotes.Where(pq => salesChannelUser.Select(sc => sc.SalesChannelID).Contains(pq.SalesChannelID)).ToList();

            return result;
        }

        /// <summary>
        /// Solo del Usuario Creador
        /// </summary>
        /// <param name="ownerUserId"></param>
        /// <param name="search"></param>
        /// <param name="isQuote"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public async Task<List<ProductQuote>> FindProductQuotesBySellerUserAsync(string ownerUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo)
        {
            var result = await productQuoteRepository.FindProductQuotesByUserIDAsync(ownerUserId, search, isQuote, dateFrom, dateTo);

            return result;
        }

        /// <summary>
        /// Solo del Usuario Creader si coincide la Empresa
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="userId"></param>
        /// <param name="search"></param>
        /// <param name="isQuote"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public async Task<List<ProductQuote>> FindProductQuotesByCustomerAndUserIDAsync(int customerID, string userId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo)
        {
            var result = await productQuoteRepository.FindProductQuotesByCustomerAndUserIDAsync(customerID, userId, search, isQuote, dateFrom, dateTo);

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (productQuoteRepository != null)
                {
                    productQuoteRepository.Dispose();
                    productQuoteRepository = null;
                }
                if (productRepository != null)
                {
                    productRepository.Dispose();
                    productRepository = null;
                    workflowMessageService = null;
                    pdfService = null;
                }
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
                if (pdfService != null)
                {
                    pdfService.Dispose();
                    pdfService = null;
                }
                if (saleModalityProductRepository != null)
                {
                    saleModalityProductRepository.Dispose();
                    saleModalityProductRepository = null;
                }
                if (customerOrderRepository != null)
                {
                    customerOrderRepository.Dispose();
                    customerOrderRepository = null;
                }
                if (saleModalityCreditRatingRepository != null)
                {
                    saleModalityCreditRatingRepository.Dispose();
                    saleModalityCreditRatingRepository = null;
                }
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }
                if (globalVariableRepository != null)
                {
                    globalVariableRepository.Dispose();
                    globalVariableRepository = null;
                }
                if (transportTypeRepository != null)
                {
                    transportTypeRepository.Dispose();
                    transportTypeRepository = null;
                }
                if (geographicAreaTransportTypeRepository != null)
                {
                    geographicAreaTransportTypeRepository.Dispose();
                    geographicAreaTransportTypeRepository = null;
                }
                if (packagingRepository != null)
                {
                    packagingRepository.Dispose();
                    packagingRepository = null;
                }
                if (rofexRepository != null)
                {
                    rofexRepository.Dispose();
                    rofexRepository = null;
                }
                if (paymentDeadlineRepository != null)
                {
                    paymentDeadlineRepository.Dispose();
                    paymentDeadlineRepository = null;
                }
                if (shipmentTrackingRepository != null)
                {
                    shipmentTrackingRepository.Dispose();
                    shipmentTrackingRepository = null;
                }
                if (customerOrderService != null)
                {
                    customerOrderService.Dispose();
                    customerOrderService = null;
                }
                if (transportServices != null)
                {
                    transportServices.Dispose();
                    transportServices = null;
                }
                if (marginServices != null)
                {
                    marginServices.Dispose();
                    marginServices = null;
                }
                if (wayOfExceptionServices != null)
                {
                    wayOfExceptionServices.Dispose();
                    wayOfExceptionServices = null;
                }
                if (salesChannelUserRepository != null)
                {
                    salesChannelUserRepository.Dispose();
                    salesChannelUserRepository = null;
                }
            }
        }

    }
}
