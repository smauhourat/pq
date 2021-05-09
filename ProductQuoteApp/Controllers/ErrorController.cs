using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController()
        {
        }

        // GET: /Error/HttpError404
        public ActionResult HttpError404(string message)
        {
            return View("SomeView", message);
        }
    }
}