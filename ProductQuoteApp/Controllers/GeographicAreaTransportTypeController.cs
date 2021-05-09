using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class GeographicAreaTransportTypeController : Controller
    {
        // GET: GeographicAreaTransportType
        [Authorize(Roles = "AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
    }
}