using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class SaleModalityProductController : Controller
    {
        // GET: SaleModalityProduct
        [Authorize(Roles = "AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
    }
}