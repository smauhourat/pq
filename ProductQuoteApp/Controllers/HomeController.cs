using ProductQuoteApp.Helpers;
using ProductQuoteApp.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class HomeController : BaseController
    {
        IWorkflowMessageService workflowMessageService = null;

        public HomeController(IWorkflowMessageService workflowMessageServ)
        {
            workflowMessageService = workflowMessageServ;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Default()
        {
            return View();
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && workflowMessageService != null)
            {
                workflowMessageService.Dispose();
                workflowMessageService = null;
            }
            base.Dispose(disposing);
        }
    }
}