using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductQuoteApp.Services;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Tests.Services
{
    [TestClass]
    public class MarginServicesTest
    {
        #region "Calculos de Margenes"

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_USD_Producto_NULL_Cliente()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginPercentage = 10;
            product.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginProduct);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginProduct);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_USD_Cliente_NULL_Producto()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            customer.MinimumMarginPercentage = 10;
            customer.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginCustomer);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginCustomer);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_USD_Default()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(5, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(50, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginSaleModality);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginSaleModality);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Producto_USD_Default()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginPercentage = 10;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(50, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginProduct);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginSaleModality);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Producto_USD_Default_PORC_ClienteNotNull()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginPercentage = 10;
            customer.MinimumMarginPercentage = 5;
            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);

            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(50, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginProduct);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginSaleModality);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Producto_USD_Cliente()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginPercentage = 10;
            customer.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginProduct);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginCustomer);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Cliente_USD_Default()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            customer.MinimumMarginPercentage = 10;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(50, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginCustomer);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginSaleModality);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Cliente_USD_Default_PORC_ProductoNotNull()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginPercentage = 5;
            customer.MinimumMarginPercentage = 10;
            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(50, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginCustomer);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginSaleModality);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Cliente_USD_Producto()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            customer.MinimumMarginPercentage = 10;
            product.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginCustomer);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginProduct);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Default_USD_Producto()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            product.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(5, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginSaleModality);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginProduct);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Default_USD_Cliente()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            customer.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(5, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(100, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginSaleModality);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginCustomer);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Input_USD_Default()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            productQuote.MargenInput = 10;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginInput);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginInput);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Input_USD_Producto()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            productQuote.MargenInput = 10;
            product.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginInput);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginInput);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Input_USD_Cliente()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();
            SaleModalityCreditRating saleModalityCreditRating = new SaleModalityCreditRating();

            saleModalityCreditRating.MinimumMarginPercentage = 5;
            saleModalityCreditRating.MinimumMarginUSD = 50;

            productQuote.MargenInput = 10;
            customer.MinimumMarginUSD = 100;

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, saleModalityCreditRating);


            // Assert
            Assert.AreEqual(10, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginInput);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginInput);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_Null_USD_Null()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();

            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, null);


            // Assert
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginUSD);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginUndefined);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourceUSD, MarginTypes.MarginUndefined);
        }

        [TestMethod()]
        public void GetMargenNetoPorcentual_PORC_USD_NULL()
        {
            // Arrange
            MarginServices marginServices = new MarginServices();
            ProductQuote productQuote = new ProductQuote();
            Product product = new Product();
            Customer customer = new Customer();


            // Act
            MininumMarginSale mininumMarginSale = marginServices.GetMargenNetoPorcentual(productQuote, product, customer, null);


            // Assert
            Assert.AreEqual(0, mininumMarginSale.MinimumMarginPercentage);
            Assert.AreEqual(mininumMarginSale.MininumMarginSourcePercentage, MarginTypes.MarginUndefined);
        }

        #endregion

    }
}
