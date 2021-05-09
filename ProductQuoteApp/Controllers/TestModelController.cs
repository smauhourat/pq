using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class TestModelController : BaseController
    {
        private ITestModelRepository testModelRepository = null;

        public TestModelController(ITestModelRepository testModelRepo)
        {
            testModelRepository = testModelRepo;

        }

        // GET: TestModel
        public ActionResult Index()
        {
            var result = testModelRepository.ListAll();
            return View(result.ToList());
        }

        // GET: TestModel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestModel/Create
        [HttpPost]
        public ActionResult Create(TestModel model)        //Create([Bind(Include = "ProviderID,ProviderName")] Provider provider)
        {

            //object actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture)
            decimal v = (decimal)1234.56;
            string r;
            r = Convert.ToDecimal(v).ToString("#,##0.00");

            int i = 1234;
            string r2;
            r2 = i.ToString("#,##0");

            //if (Decimal.TryParse(valueProvider.AttemptedValue, NumberStyles.Currency, new CultureInfo(culture), out value))
            //{
            //    return value;
            //}

            if (ModelState.IsValid)
            {
                testModelRepository.Create(model);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // GET: TestModel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestModel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TestModel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestModel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && testModelRepository != null)
            {
                testModelRepository.Dispose();
                testModelRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
