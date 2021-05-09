using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProductQuoteApp.Controllers;
using ProductQuoteApp.Logging;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Tests.Controllers
{
    [TestClass]
    public class CreditRatingControllerTest
    {
        [TestMethod]
        public async Task When_CallFindCreditRatingsAsync_Then_ReturnsAll()
        {
            // Arrange
            CreditRatingController controller = new CreditRatingController(new CreditRatingRepository(new Logger()));

            // Act
            var result = await controller.Index() as ViewResult;

            controller.Dispose();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
            var data = (IEnumerable<CreditRating>)result.ViewData.Model;
            Assert.AreEqual(7, data.Count());
        }

        //FindCreditRatingByIDAsync
        [TestMethod]
        public async Task When_CallFindCreditRatingByIDAsync_Then_ReturnsOne()
        {
            // Arrange
            CreditRatingController controller = new CreditRatingController(new CreditRatingRepository(new Logger()));

            // Act
            var result = await controller.Details(1) as ViewResult;

            controller.Dispose();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
        }
    }
}
