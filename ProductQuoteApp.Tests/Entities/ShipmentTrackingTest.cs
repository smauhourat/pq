using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductQuoteApp.Services;
using ProductQuoteApp.Persistence;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Tests.Entities
{
    [TestClass]
    public class ShipmentTrackingTest
    {
        [TestMethod()]
        public void When_CustomerOrderCompleted_And_NotQuotedCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                CustomerOrderCompleted = true,
                QuotedCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Orden de Compra' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ApprovedCompleted_And_NotCustomerOrderCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ApprovedCompleted = true,
                CustomerOrderCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Aprobado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_InProductionEnabled_And_InProductionCompleted_And_NotApprovedCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                InProductionEnabled = true,
                InProductionCompleted = true,
                ApprovedCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'En Px' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETDEnabled_And_ETDCompleted_And_InProductionEnabled_And_NotInProductionCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDCompleted = true,
                InProductionEnabled = true,
                ApprovedCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Embarcado ETD' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETDEnabled_And_ETDCompleted_And_NotInProductionEnabled_And_NotApprovedCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDCompleted = true,
                InProductionEnabled = false,
                ApprovedCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Embarcado ETD' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETACompleted_And_ETDEnabled_And_NotETDCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETACompleted = true,
                ETDEnabled = true,
                ETDCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Puerto ETA' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETACompleted_And_InProductionEnabled_And_NotInProductionCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETACompleted = true,
                InProductionEnabled = true,
                InProductionCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Puerto ETA' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabledvAnd_NationalizedCompleted_And_ETAEnabled_And_NotETACompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedCompleted = true,
                ETAEnabled = true,
                ETACompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedCompleted_And_ETDEnabled_And_NotETDCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedCompleted = true,
                ETDEnabled = true,
                ETDCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedCompleted_And_InProductionEnabled_And_NotInProductionCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedCompleted = true,
                InProductionEnabled = true,
                InProductionCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredCompleted_And_NationalizedEnabled_And_NotNationalizedCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredCompleted = true,
                NationalizedEnabled = true,
                NationalizedCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredCompleted_And_ETAEnabled_And_NotETACompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredCompleted = true,
                ETAEnabled = true,
                ETACompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredCompleted_And_ETDEnabled_And_NotETDCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredCompleted = true,
                ETDEnabled = true,
                ETDCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredCompleted_And_InProductionEnabled_And_NotInProductionCompleted_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredCompleted = true,
                InProductionEnabled = true,
                InProductionCompleted = false
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ApprovedEstimatedDate_LessThan_CustomerOrderRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ApprovedEstimatedDate = DateTime.Now.AddDays(-1),
                CustomerOrderRealDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Aprobado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_InProductionEnabled_And_InProductionEstimatedDate_LessThan_ApprovedRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                InProductionEnabled = true,
                InProductionEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedRealDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'En Px' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_InProductionEnabled_And_InProductionEstimatedDate_LessThan_ApprovedEstimatedDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                InProductionEnabled = true,
                InProductionEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedEstimatedDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'En Px' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETDEnabled_And_ETDEstimatedDate_LessThan_InProductionRealDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionRealDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETDEnabled_And_ETDEstimatedDate_LessThan_ApprovedRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedRealDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETDEnabled_And_ETDEstimatedDate_LessThan_ApprovedEstimatedDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedEstimatedDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_ETDRealDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                ETDRealDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_ETDEstimatedDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                ETDEstimatedDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_InProductionRealDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionRealDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_InProductionEstimatedDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionEstimatedDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_ApprovedRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedRealDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ETAEnabled_And_ETAEstimatedDate_LessThan_ApprovedEstimatedDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETAEnabled = true,
                ETAEstimatedDate = DateTime.Now.AddDays(-1),
                ApprovedEstimatedDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_ETARealDate_And_ETAEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                ETARealDate = DateTime.Now,
                ETAEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_ETAEstimatedDate_And_ETAEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                ETAEstimatedDate = DateTime.Now,
                ETAEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_ETDRealDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                ETDRealDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_ETDEstimatedDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                ETDEstimatedDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_InProductionRealDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionRealDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_NationalizedEnabled_And_NationalizedEstimatedDate_LessThan_InProductionEstimatedDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                NationalizedEnabled = true,
                NationalizedEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionEstimatedDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_NationalizedRealDate_And_NationalizedEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                NationalizedRealDate = DateTime.Now,
                NationalizedEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_NationalizedEstimatedDate_And_NationalizedEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                NationalizedEstimatedDate = DateTime.Now,
                NationalizedEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_ETARealDate_And_ETAEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                ETARealDate = DateTime.Now,
                ETAEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_ETAEstimatedDate_And_ETAEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                ETAEstimatedDate = DateTime.Now,
                ETAEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_ETDRealDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                ETDRealDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_ETDEstimatedDate_And_ETDEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                ETDEstimatedDate = DateTime.Now,
                ETDEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_InProductionRealDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionRealDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_DeliveredEstimatedDate_LessThan_InProductionEstimatedDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                DeliveredEstimatedDate = DateTime.Now.AddDays(-1),
                InProductionEstimatedDate = DateTime.Now,
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod()]
        public void When_ApprovedRealDate_LessThan_CustomerOrderRealDate_And_ApprovedRealDateNotNull_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ApprovedRealDate = DateTime.Now.AddDays(-1),
                CustomerOrderRealDate = DateTime.Now
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Real de 'Aprobado' no puede ser menor a la Fecha Real de la la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod]
        public void When_InProductionEnabled_And_InProductionRealDateNotNull_And_InProductionRealDate_LessThan_ApprovedRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                InProductionEnabled = true,
                ApprovedRealDate = DateTime.Now,
                InProductionRealDate = DateTime.Now.AddDays(-1)
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Real de 'En Px' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod]
        public void When_InProductionEnabled_And_InProductionRealDateNotNull_And_ApprovedRealDateIsNll_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                InProductionEnabled = true,
                InProductionRealDate = DateTime.Now,
                ApprovedRealDate = null
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Real de 'En Px' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod]
        public void When_ETDEnabled_And_ETDRealDateNotNull_And_ETDRealDate_LessThan_InProductionRealDate_And_InProductionEnabled_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                InProductionRealDate = DateTime.Now,
                ETDRealDate = DateTime.Now.AddDays(-1),
                ApprovedRealDate = DateTime.Now.AddDays(-1),
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Real de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        [TestMethod]
        public void When_ETDEnabled_And_ETDRealDateNotNull_And_InProductionRealDate_LessThan_ApprovedRealDate_And_InProductionEnabled_And_ETDRealDate_GreatherThan_InProductionRealDate_Then_ThrowValidationError()
        {
            // Arrange
            ShipmentTracking shipmentTracking = new ShipmentTracking
            {
                ETDEnabled = true,
                ETDRealDate = DateTime.Now,
                ApprovedRealDate = DateTime.Now,
                InProductionRealDate = DateTime.Now.AddDays(-1),                
                InProductionEnabled = true
            };

            // Act
            ValidationResult result = ShipmentTrackingRules.ValidateShipmentTracking(shipmentTracking, null);

            // Assert
            Assert.AreEqual("La Fecha Real de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior", result.ErrorMessage);
        }

        //Voy por 234
    }
}
