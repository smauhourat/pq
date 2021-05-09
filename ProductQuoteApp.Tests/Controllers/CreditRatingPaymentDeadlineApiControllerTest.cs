using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProductQuoteApp.Controllers;
using ProductQuoteApp.Logging;
using ProductQuoteApp.Persistence;
using System.Threading.Tasks;

namespace ProductQuoteApp.Tests.Controllers
{
    [TestClass]
    public class CreditRatingPaymentDeadlineApiControllerTest
    {
        [TestMethod]
        public async Task When_CallGetCreditRatings_Then_ReturnsAll()
        {
            // Arrange
            CreditRatingPaymentDeadlineApiController controller = new CreditRatingPaymentDeadlineApiController(new CreditRatingRepository(new Logger()), new CreditRatingPaymentDeadlineRepository(new Logger()));
            string resultAsJsonExpected = "[{\"CreditRatingID\":1,\"Description\":\"AAAA - Sin limite de Crédito\"},{\"CreditRatingID\":2,\"Description\":\"AA - Hasta 90 días de Crédito\"},{\"CreditRatingID\":4,\"Description\":\"A - Hasta 60 días de Crédito\"},{\"CreditRatingID\":6,\"Description\":\"B - Hasta 45 días de Crédito\"},{\"CreditRatingID\":7,\"Description\":\"C - Hasta 30 días de Crédito\"},{\"CreditRatingID\":8,\"Description\":\"D - Sin Crédito\"},{\"CreditRatingID\":9,\"Description\":\"AAA-Hasta 120 días de Crédito\"}]";

            // Act
            var result = await controller.GetCreditRatingsMM();
            var resultAsJson = JsonConvert.SerializeObject(result);

            controller.Dispose();

            // Assert
            Assert.AreEqual(7, result.Count);
            Assert.AreEqual(resultAsJsonExpected, resultAsJson);
        }

        [TestMethod]
        public async Task When_CallGetPaymentDeadlineByCreditRatingApi_Then_ReturnsOnlyTrue()
        {
            // Arrange
            CreditRatingPaymentDeadlineApiController controller = new CreditRatingPaymentDeadlineApiController(new CreditRatingRepository(new Logger()), new CreditRatingPaymentDeadlineRepository(new Logger()));
            string resultAsJsonExpected = "[{\"CreditRatingPaymentDeadlineID\":1,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":1,\"PaymentDeadlineDescription\":\"Contado Anticipado\"},{\"CreditRatingPaymentDeadlineID\":6,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":3,\"PaymentDeadlineDescription\":\"30 Días FF\"},{\"CreditRatingPaymentDeadlineID\":7,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":2,\"PaymentDeadlineDescription\":\"Contado 7 Días\"},{\"CreditRatingPaymentDeadlineID\":12,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":4,\"PaymentDeadlineDescription\":\"45 Días FF\"},{\"CreditRatingPaymentDeadlineID\":14,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":5,\"PaymentDeadlineDescription\":\"60 Días FF\"},{\"CreditRatingPaymentDeadlineID\":16,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":6,\"PaymentDeadlineDescription\":\"90 Días FF\"},{\"CreditRatingPaymentDeadlineID\":56,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":11,\"PaymentDeadlineDescription\":\"120 dias F.F.\"},{\"CreditRatingPaymentDeadlineID\":57,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":12,\"PaymentDeadlineDescription\":\"180 días F.F.\"},{\"CreditRatingPaymentDeadlineID\":58,\"CreditRatingID\":1,\"CreditRatingDescription\":\"AAAA - Sin limite de Crédito\",\"PaymentDeadlineID\":13,\"PaymentDeadlineDescription\":\"70 Días FF\"}]";

            // Act
            var result = await controller.GetPaymentDeadlineByCreditRatingApi(1);
            var resultAsJson = JsonConvert.SerializeObject(result);

            controller.Dispose();

            // Assert
            Assert.AreEqual(9, result.Count);
            Assert.AreEqual(resultAsJsonExpected, resultAsJson);
        }

        [TestMethod]
        public void When_CallGetPaymentDeadlineAvailables_Then_ReturnsOnlyTrue()
        {
            // Arrange
            CreditRatingPaymentDeadlineApiController controller = new CreditRatingPaymentDeadlineApiController(new CreditRatingRepository(new Logger()), new CreditRatingPaymentDeadlineRepository(new Logger()));
            string resultAsJsonExpected = "[{\"PaymentDeadlineID\":11,\"Description\":\"120 dias F.F.\",\"Days\":120,\"Months\":4.0000000000},{\"PaymentDeadlineID\":12,\"Description\":\"180 días F.F.\",\"Days\":180,\"Months\":6.0000000000}]";

            // Act
            var result = controller.GetPaymentDeadlineAvailables(2);
            var resultAsJson = JsonConvert.SerializeObject(result);

            controller.Dispose();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(resultAsJsonExpected, resultAsJson);
        }
    }
}
