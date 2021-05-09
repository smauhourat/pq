using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ProductQuoteApp;
using ProductQuoteApp.Controllers;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Logging;

namespace ProductQuoteApp.Tests.Controllers
{
    [TestClass]
    public class ProviderControllerTest
    {
        //[TestMethod]
        //public void Index()
        //{
        //    // Arrange 
        //    ProviderController controller = new ProviderController(new ProviderRepository(new Logger()));

        //    // Act
        //    Task<ActionResult> result = controller.Index() as Task<ActionResult>;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public async Task Details()
        //{
        //    // Arrange 
        //    ProviderController controller = new ProviderController(new ProviderRepository(new Logger()));

        //    // Act
        //    var result = await controller.Details(1) as ViewResult;

        //    var provider = (Provider) result.ViewData.Model;

        //    // Assert
        //    Assert.AreEqual("Inquimex SACI", provider.ProviderName);
        //}

        //[TestMethod]
        //public async Task Create()
        //{
        //    // Arrange 
        //    ProviderController controller = new ProviderController(new ProviderRepository(new Logger()));

        //    // Act
        //    Provider provider = new Provider();
        //    provider.ProviderID = 99;
        //    provider.ProviderName = "Test 99";

        //    var result = await controller.Create(provider) as ViewResult;

        //    // Assert
        //    //Assert.AreEqual("Proveedor 1", provider.ProviderName);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.ViewData.ModelState.Count);
        //}

    }
}
