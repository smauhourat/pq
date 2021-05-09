using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class SaleModalityCreditRatingController : Controller
    {
        [Authorize(Roles = "AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
    }
}