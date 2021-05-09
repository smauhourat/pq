using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductQuoteApp.Services;
using ProductQuoteApp.Persistence;

//https://wrightfully.com/assert-framework-comparison

namespace ProductQuoteApp.Tests.Services
{
    [TestClass]
    public class ProductQuoteServicesTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region "Caculos ROFEX"

        [TestMethod]
        public void GetRofexNuevo_ConValorDiasAtrasoCliente1()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofexNuevo(1, 45);

            // Assert
            Assert.AreEqual(60, rofex.Days);
        }

        [TestMethod]
        public void GetRofexNuevo_ConValorDiasAtrasoCliente2()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofexNuevo(1, 25);

            // Assert
            Assert.AreEqual(30, rofex.Days);
        }

        [TestMethod]
        public void GetRofexNuevo_ConValorDiasAtrasoCliente3()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofexNuevo(2, 25);

            // Assert
            Assert.AreEqual(60, rofex.Days);
        }

        [TestMethod]
        public void GetRofexNuevo_ConValorDiasAtrasoCliente4()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofexNuevo(3, 0);

            // Assert
            Assert.AreEqual(30, rofex.Days);
        }

        [TestMethod]
        public void GetRofexNuevo_ConValorDiasAtrasoCliente5()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofexNuevo(1, 0);

            // Assert
            Assert.AreEqual(null, rofex);
        }

        [TestMethod]
        public void GetRofex_ConValorDiasAtrasoCliente()
        {
            // Arrange
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            Rofex rofex = productQuoteService.GetRofex(1);

            // Assert
            Assert.AreEqual(null, rofex);
        }

        #endregion


        #region "Validaciones Input Calculo Cotizacion"

        [TestMethod]
        public void Cuando_CalculamosCon_ProductoNull_DeberiaRetornarMensaje()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();


            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuoteService.CalculateQuote(productQuote);

            // Assert
            Assert.AreEqual("Debe ingresar un Producto", productQuote.Message);
        }

        [TestMethod]
        public void Cuando_CalculamosCon_QuantityOpenPurchaseOrderEqualOrLessThanZero_And_SaleModalityIsLocal_DeberiaRetornarMensaje()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();


            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 1;
            productQuote.SaleModalityID = (int)EnumSaleModality.Local;
            productQuoteService.CalculateQuote(productQuote);

            // Assert
            Assert.AreEqual("Debe ingresar Cantidad Total  Orden de Compra (Kg)", productQuote.Message);
        }

        [TestMethod]
        public void Cuando_CalculamosCon_MinimumQuantityDeliveryNull_DeberiaRetornarMensaje()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();


            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 1;
            productQuote.SaleModalityID = (int)EnumSaleModality.LocalProgramada;
            productQuoteService.CalculateQuote(productQuote);

            // Assert
            Assert.AreEqual("Debe seleccionar la Cantidad de Entregas.", productQuote.Message);
        }

        [TestMethod]
        public void Cuando_CalculamosCon_MinimumQuantityDelivery_MayorQue_QuantityOpenPurchaseOrder_DeberiaRetornarMensaje()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 1;
            productQuote.SaleModalityID = (int)EnumSaleModality.LocalProgramada;
            productQuote.MinimumQuantityDelivery = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            Assert.AreEqual("La Cantidad Minima por Entrega (Kg) no puede ser mayor a la Cantidad Total  Orden de Compra (Kg)", productQuote.Message);
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaIndentSL_Entonces_CantidadMinimaPorEntrega_DebeSer_Menor_CantidadTotalOrdenCompra()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 1;
            productQuote.SaleModalityID = (int)EnumSaleModality.IndentSL;
            productQuote.MinimumQuantityDelivery = 10;
            productQuote.MaximumMonthsStock = 0;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            Assert.AreEqual("La Cantidad Minima por Entrega (Kg) no puede ser mayor a la Cantidad Total  Orden de Compra (Kg)", productQuote.Message);
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaLocalProgramada_Entonces_CantidadTotalOrdenCompra_DebeSer_Multiplo_ProductFCLKilogram()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            productQuote.SaleModalityID = (int)EnumSaleModality.LocalProgramada;
            productQuote.MinimumQuantityDelivery = 10;
            productQuote.QuantityOpenPurchaseOrder = 30000;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "Para la modalidad Distribución con Importación a Pedido, la Cantidad Total  Orden de Compra (Kg) debe ser multiplo de la Capacidad del FCL:");
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaIndentSL_Entonces_CantidadTotalOrdenCompra_DebeSer_Multiplo_CapacidadFCL()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            productQuote.SaleModalityID = (int)EnumSaleModality.IndentSL;
            productQuote.MinimumQuantityDelivery = 10;
            productQuote.QuantityOpenPurchaseOrder = 30000;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "Para la modalidad Indent / Indent + Servicio Logistico/Financiero de guarda, la Cantidad Total  Orden de Compra (Kg) debe ser multiplo de la Capacidad del FCL:");
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaLocal_Entonces_CantidadTotalOrdenDeCompra_DebeSer_Multiplo_UnidadMínimaVentaProducto()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            //product.PositionKilogram = 1250
            productQuote.SaleModalityID = (int)EnumSaleModality.Local;
            productQuote.MinimumQuantityDelivery = 1;
            productQuote.QuantityOpenPurchaseOrder = 999;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "La Cantidad Total  Orden de Compra (Kg) debe ser multiplo de Unidad Mínima de Venta del Producto");
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaLocal_Entonces_CantidadTotal_DebeSer_MultiploPosicionKgProducto()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            //product.PositionKilogram = 1250
            productQuote.SaleModalityID = (int)EnumSaleModality.Local;
            productQuote.MinimumQuantityDelivery = 1;
            productQuote.QuantityOpenPurchaseOrder = 999;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "La Cantidad Total  Orden de Compra (Kg) debe ser multiplo de Unidad Mínima de Venta del Producto");
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaLocal_Entonces_CantidadTotalDivididoCantidadEntregas_DebeSer_Multiplo_UnidadMinimaVentaProducto()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            //product.PositionKilogram = 1250
            productQuote.SaleModalityID = (int)EnumSaleModality.Local;
            productQuote.MinimumQuantityDelivery = 1;
            productQuote.QuantityOpenPurchaseOrder = 999;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "La Cantidad Total  Orden de Compra (Kg) debe ser multiplo de Unidad Mínima de Venta del Producto");
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaIndent_Entonces_CantidadMinima_DebeSer_Multiplo_CantidadFCLProductoToneladas()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();

            // Act
            ProductQuote productQuote = new ProductQuote();
            productQuote.ProductID = 2;
            //product.FCLKilogram = 26250
            //product.PositionKilogram = 1250
            productQuote.SaleModalityID = (int)EnumSaleModality.Indent;
            productQuote.MinimumQuantityDelivery = 30000;
            productQuote.QuantityOpenPurchaseOrder = 60000;
            productQuote.MaximumMonthsStock = 10;

            productQuoteService.CalculateQuote(productQuote);

            // Assert
            StringAssert.Contains(productQuote.Message, "Para la modalidad Indent, la Cantidad Minima de la OC (Kg) debe ser multiplo de la capacidad del FCL");

        }

        private void SetProductToProductQuote(ProductQuote productQuote, Product product)
        {
            //Producto
            productQuote.ProductID = product.ProductID; //5: Agua Amoniacal 28%
            productQuote.ProductName = product.FullName;
            productQuote.ProductProviderName = product.Provider.ProviderName;
            productQuote.ProductBrandName = product.Brand;
            productQuote.ProductFCLKilogram = product.FCLKilogram.ToString();
            productQuote.ProductBuyAndSellDirect = product.BuyAndSellDirect;
            productQuote.ProductSingleName = product.Name;
            productQuote.ProductPackagingID = product.PackagingID;
            productQuote.ProductPackagingName = product.Packaging.Description;
            productQuote.ProductValidityOfPrice = product.ValidityOfPrice;
            productQuote.ProductInOutStorage = product.InOutStorage;

        }

        private void SetCustomerToProductQuote(ProductQuote productQuote, Customer customer)
        {
            productQuote.CustomerID = customer.CustomerID; //Agrofacil S.A.
            productQuote.CustomerName = "";
            productQuote.CustomerContactMail = customer.ContactEmail;
            productQuote.CustomerCompany = customer.Company;
            productQuote.CustomerDelayAverageDays = customer.DelayAverageDays;
        }

        [TestMethod]
        public void Cuando_CalculamosCon_ModalidadVentaLocal_TodoOK()
        {
            // Arrange 
            ProductQuoteServiceBuilder builder = new ProductQuoteServiceBuilder();
            IProductQuoteService productQuoteService = builder.Build();

            IProductRepository productRepository = builder.ProductRepositoryCreate();
            ICustomerRepository customerRepository = builder.CustomerRepositoryCreate();

            // Act
            ProductQuote productQuote = new ProductQuote();
            Product product = productRepository.FindProductsByID(5); //5: Agua Amoniacal 28%
            Customer customer = customerRepository.FindCustomersByID(22); //22: Agrofacil S.A.

            //Producto
            SetProductToProductQuote(productQuote, product);

            //Cliente
            SetCustomerToProductQuote(productQuote, customer);

            //Modalidad de Venta
            productQuote.SaleModalityID = (int)EnumSaleModality.Local; //Distribucion Local
            productQuote.SaleModalityName = "Distribución Local";

            //Lugar de Entrega
            productQuote.GeographicAreaID = 7; //A Retirar
            productQuote.GeographicAreaName = "A Retirar";

            //Condicion de Pago
            productQuote.PaymentDeadlineID = 1; //Contado Anticipado
            productQuote.PaymentDeadlineName = "Contado Anticipado";

            //Cantidad Total Orden Compra (Kg)
            productQuote.QuantityOpenPurchaseOrder = 28000;

            //Cantidad de Entregas
            productQuote.DeliveryAmount = 1; //1 Entrega

            //Cantidad Minima por Entrega (Kg)
            productQuote.MinimumQuantityDelivery = 28000;

            //Máximo Tiempo en Stock (Meses)
            productQuote.MaximumMonthsStock = 0; //Entrega Inmediata

            //Moneda de la Operación
            productQuote.ExchangeTypeID = 1; //Dolares
            productQuote.ExchangeTypeName = "Dólares";
            productQuote.ExchangeTypeLargeDescription = "Con variación Tipo Cambio";

            productQuote.MargenNetoEntidadOrigen = "0";

            productQuote.GVD_CostoAlmacenamientoMensual = 400;
            productQuote.GVD_CostoFinancieroMensual = 1;
            productQuote.GVD_CostoInOut = 800;
            productQuote.GVD_DiasStockPromedioDistLocal = 60;
            productQuote.GVD_FactorCostoAlmacenamientoMensual = 2;
            productQuote.GVD_GastosFijos = 5;
            productQuote.GVD_IIBBAlicuota = 0;
            productQuote.GVD_ImpuestoDebitoCredito = 0;
            productQuote.GVD_TipoCambio = 72;


            //Inicializaciones
            productQuote.FreightType = "0";
            productQuote.CostoFinancieroMensual_PorITEM_Formula = "";

            productQuote.IsSellerUser = true;
            productQuote.ProductSellerCompanyName = "Inquimex S.A";

            var jsonProductQuotePre = Newtonsoft.Json.JsonConvert.SerializeObject(productQuote);
            System.Diagnostics.Debug.WriteLine("PRE: " + jsonProductQuotePre);

            productQuoteService.CalculateQuote(productQuote);

            var jsonProductQuotePost = Newtonsoft.Json.JsonConvert.SerializeObject(productQuote);
            System.Diagnostics.Debug.WriteLine("POST: " + jsonProductQuotePost);

            // Assert
            Assert.IsFalse(productQuote.Price <= 0);
        }



        //[TestMethod]
        //public void Cuando_Calculamos_FactorCostoAlmacenamientoMensual_DebeSer_Mayor_Cero()
        //{
        //    // Arrange 
        //    IProductQuoteService productQuoteService = new ProductQuoteServiceNewBuilder().Build();

        //    // Act
        //    ProductQuote productQuote = new ProductQuote();
        //    productQuote.ProductID = 2;
        //    //product.FCLKilogram = 26250
        //    //product.PositionKilogram = 1250
        //    productQuote.SaleModalityID = (int)EnumSaleModality.Indent;
        //    productQuote.MinimumQuantityDelivery = 26250;
        //    productQuote.QuantityOpenPurchaseOrder = 26250*2;
        //    productQuote.MaximumMonthsStock = 10;

        //    productQuoteService.CalculateQuote(productQuote);

        //    //Falta poner el GVD_FactorCostoAlmacenamientoMensual en cero

        //    // Assert
        //    StringAssert.Contains(productQuote.Message, "El Valor del 'Factor Tiempo en Stock' debe ser mayor a cero.");

        //}

        //[TestMethod]
        //public void Cuando_Calculamos_TipoCambio_DebeSer_Mayor_Cero()
        //{
        //    // Arrange 
        //    IProductQuoteService productQuoteService = new ProductQuoteServiceNewBuilder().Build();

        //    // Act
        //    ProductQuote productQuote = new ProductQuote();
        //    productQuote.ProductID = 2;
        //    //product.FCLKilogram = 26250
        //    //product.PositionKilogram = 1250
        //    productQuote.SaleModalityID = (int)EnumSaleModality.Indent;
        //    productQuote.MinimumQuantityDelivery = 26250;
        //    productQuote.QuantityOpenPurchaseOrder = 26250*2;
        //    productQuote.MaximumMonthsStock = 10;

        //    productQuoteService.CalculateQuote(productQuote);

        //    //Falta poner el GVD_TipoCambio en cero

        //    // Assert
        //    StringAssert.Contains(productQuote.Message, "El Valor del 'Tipo Cambio (ARS/USD)' debe ser mayor a cero.");

        //}
        #endregion

        [TestMethod]
        public void Cuando_CalculamosDirectamente_DeberiaCalcularOK()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();
            ProductQuote productQuote = new ProductQuote();
            productQuote.CustomerCompany = "Empresa Cliente";
            productQuote.CustomerContactMail = "santiagomauhourat@hotmail.com";
            productQuote.CustomerContactName = "Contacto Cliente";
            productQuote.CustomerID = 9;
            productQuote.CostoFinancieroMensual_PorITEM_Formula = "";
            productQuote.CustomerName = "";
            productQuote.DeliveryAmount = 1;
            productQuote.ExchangeTypeID = 1;
            productQuote.ExchangeTypeLargeDescription = "Con variación Tipo Cambio";
            productQuote.ExchangeTypeName = "Dólares";
            productQuote.ExpressCalc = true;
            productQuote.FreightType = "0";

            productQuote.GVD_CostoAlmacenamientoMensual = 400;
            productQuote.GVD_CostoFinancieroMensual = 1;
            productQuote.GVD_CostoInOut = 800;
            productQuote.GVD_DiasStockPromedioDistLocal = 60;
            productQuote.GVD_FactorCostoAlmacenamientoMensual = 2;
            productQuote.GVD_GastosFijos = 5;
            productQuote.GVD_TipoCambio = 72;

            productQuote.GeographicAreaID = 7;
            productQuote.GeographicAreaName = "A Retirar";
            productQuote.IsSellerUser = true;
            productQuote.LeyendaCalculoCostoTransporte = "";
            productQuote.MargenNetoEntidadOrigen = "0";
            productQuote.MaximumMonthsStock = 0;
            productQuote.Message = "";
            productQuote.MinimumQuantityDelivery = 28000;
            productQuote.PaymentDeadlineID = 1;
            productQuote.PaymentDeadlineMonths = 0;
            productQuote.PaymentDeadlineName = "Contado Anticipado";
            productQuote.ProductBrandName = "Porfertil ";
            productQuote.ProductBuyAndSellDirect = true;
            productQuote.ProductFCLKilogram = "28000";
            productQuote.ProductID = 1032;
            productQuote.ProductInOutStorage = false;
            productQuote.ProductName = "Urea Industrial Premium - Porfertil  - Granel";
            productQuote.ProductPackagingID = 5;
            productQuote.ProductPackagingName = "Granel";
            productQuote.ProductProviderName = "Profertil S.A.";
            productQuote.ProductProviderPaymentDeadline = 30;
            productQuote.ProductSellerCompanyName = "Inquimex S.A";
            productQuote.ProductSingleName = "Urea Industrial Premium";
            productQuote.ProductValidityOfPrice = new DateTime(2022, 8, 1);
            productQuote.ProductValidityOfPriceInput = new DateTime(2021, 3, 26);
            productQuote.ProductWaste = 0;
            productQuote.QuantityOpenPurchaseOrder = 28000;
            productQuote.SaleModalityID = 1;
            productQuote.SaleModalityName = "Distribución Local";
            productQuote.SalesChannelID = 3;
            productQuote.TiempoMedioStockDias_Formula = "";
            productQuote.UserFullName = "Administrador";
            productQuote.UserId = "c2fbb2e1-52d4-4f79-b66b-e35ad3958a36";
            productQuote.WorkingCapital_Formula = "";

            // Act
            productQuoteService.CalculateQuote(productQuote);

            // Assert
            double precision = 1e-6;
            Assert.AreEqual(0.374d, (double)productQuote.CostoProducto_PorTON, precision);
            Assert.AreEqual(0.4155555555555555555555555556d, (double)productQuote.CustomerTotalCost, precision);
            Assert.AreEqual(0.374d, (double)productQuote.CostoProducto_PorTON, precision);
            Assert.AreEqual(0.0230864197530864197530864198d, (double)productQuote.GastosFijos_PorTON, precision);
            Assert.AreEqual(5.0d, (double)productQuote.IIBBAlicuota_PorITEM, precision);
            Assert.AreEqual(0.0230864197530864197530864198d, (double)productQuote.IIBBAlicuota_PorTON, precision);
            Assert.AreEqual(10.0d, (double)productQuote.MargenNetoOriginalPorc_PorTON, precision);
            Assert.AreEqual(10.0d, (double)productQuote.MargenNetoPorc_PorTON, precision);
            Assert.AreEqual(0.0461728395061728395061728395d, (double)productQuote.MargenNetoUSD_PorTON, precision);
            Assert.AreEqual(0.4617283950617283950617283951d, (double)productQuote.PrecioVentaRofex, precision);

            Assert.AreEqual(0.4617283950617283950617283951d, (double)productQuote.Price, precision);
            Assert.AreEqual(0.4617283950617283950617283951d, (double)productQuote.PriceOriginal, precision);
        }

        [TestMethod]
        public void Cuando_CalculamosConMargenUSD_DeberiaCalcularOK()
        {
            // Arrange 
            IProductQuoteService productQuoteService = new ProductQuoteServiceBuilder().Build();
            ProductQuote productQuote = new ProductQuote();
            productQuote.CustomerCompany = "Empresa Cliente";
            productQuote.CustomerContactMail = "santiagomauhourat@hotmail.com";
            productQuote.CustomerContactName = "Contacto Cliente";
            productQuote.CustomerID = 9;
            productQuote.CostoFinancieroMensual_PorITEM_Formula = "";
            productQuote.CustomerName = "";
            productQuote.DeliveryAmount = 1;
            productQuote.ExchangeTypeID = 1;
            productQuote.ExchangeTypeLargeDescription = "Con variación Tipo Cambio";
            productQuote.ExchangeTypeName = "Dólares";
            productQuote.ExpressCalc = true;
            productQuote.FreightType = "0";

            productQuote.GVD_CostoAlmacenamientoMensual = 400;
            productQuote.GVD_CostoFinancieroMensual = 1;
            productQuote.GVD_CostoInOut = 800;
            productQuote.GVD_DiasStockPromedioDistLocal = 60;
            productQuote.GVD_FactorCostoAlmacenamientoMensual = 2;
            productQuote.GVD_GastosFijos = 5;
            productQuote.GVD_TipoCambio = 72;

            productQuote.GeographicAreaID = 7;
            productQuote.GeographicAreaName = "A Retirar";
            productQuote.IsSellerUser = true;
            productQuote.LeyendaCalculoCostoTransporte = "";
            productQuote.MargenNetoEntidadOrigen = "0";

            //MARGEN USD INPUT
            //productQuote.MargenUSDInput = (decimal)1.6;
            productQuote.MargenUSDInput = (decimal)44800;

            productQuote.MaximumMonthsStock = 0;
            productQuote.Message = "";
            productQuote.MinimumQuantityDelivery = 28000;
            productQuote.PaymentDeadlineID = 1;
            productQuote.PaymentDeadlineMonths = 0;
            productQuote.PaymentDeadlineName = "Contado Anticipado";
            productQuote.ProductBrandName = "Porfertil ";
            productQuote.ProductBuyAndSellDirect = true;
            productQuote.ProductFCLKilogram = "28000";
            productQuote.ProductID = 1032;
            productQuote.ProductInOutStorage = false;
            productQuote.ProductName = "Urea Industrial Premium - Porfertil  - Granel";
            productQuote.ProductPackagingID = 5;
            productQuote.ProductPackagingName = "Granel";
            productQuote.ProductProviderName = "Profertil S.A.";
            productQuote.ProductProviderPaymentDeadline = 30;
            productQuote.ProductSellerCompanyName = "Inquimex S.A";
            productQuote.ProductSingleName = "Urea Industrial Premium";
            productQuote.ProductValidityOfPrice = new DateTime(2022, 8, 1);
            productQuote.ProductValidityOfPriceInput = new DateTime(2021, 3, 26);
            productQuote.ProductWaste = 0;
            productQuote.QuantityOpenPurchaseOrder = 28000;
            productQuote.SaleModalityID = 1;
            productQuote.SaleModalityName = "Distribución Local";
            productQuote.SalesChannelID = 3;
            productQuote.TiempoMedioStockDias_Formula = "";
            productQuote.UserFullName = "Administrador";
            productQuote.UserId = "c2fbb2e1-52d4-4f79-b66b-e35ad3958a36";
            productQuote.WorkingCapital_Formula = "";

            // Act
            productQuoteService.CalculateQuote(productQuote);

            // Assert
            double precision = 1e-6;
            Assert.AreEqual(0.374d, (double)productQuote.CostoProducto_PorTON, precision);
            Assert.AreEqual(0.4155555555555555555555555556d, (double)productQuote.CustomerTotalCost, precision);
            Assert.AreEqual(0.374d, (double)productQuote.CostoProducto_PorTON, precision);
            Assert.AreEqual(0.0230864197530864197530864198d, (double)productQuote.GastosFijos_PorTON, precision);
            Assert.AreEqual(5.0d, (double)productQuote.IIBBAlicuota_PorITEM, precision);
            Assert.AreEqual(0.0230864197530864197530864198d, (double)productQuote.IIBBAlicuota_PorTON, precision);
            Assert.AreEqual(79.38d, (double)productQuote.MargenNetoOriginalPorc_PorTON, precision);

            Assert.AreEqual(1.6d, (double)productQuote.MargenNetoOriginalUSD_PorTON, precision);
            Assert.AreEqual(79.38d, (double)productQuote.MargenNetoPORCRofex, precision);


            Assert.AreEqual(10.0d, (double)productQuote.MargenNetoPorc_PorTON, precision);
            Assert.AreEqual(1.6d, (double)productQuote.MargenNetoUSDRofex, precision);
            

            Assert.AreEqual(1.6d, (double)productQuote.MargenNetoUSD_PorTON, precision);
            Assert.AreEqual(2.0155555555555555555555555556d, (double)productQuote.PrecioVentaRofex, precision);
            Assert.AreEqual(2.0155555555555555555555555556d, (double)productQuote.Price, precision);
            Assert.AreEqual(2.0155555555555555555555555556d, (double)productQuote.PriceOriginal, precision);
        }

    }
}
