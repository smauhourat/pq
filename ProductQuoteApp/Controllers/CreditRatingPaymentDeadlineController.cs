using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class CreditRatingPaymentDeadlineController : Controller
    {
        // GET: CreditRatingPaymentDeadline
        [Authorize(Roles = "AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
    }
}