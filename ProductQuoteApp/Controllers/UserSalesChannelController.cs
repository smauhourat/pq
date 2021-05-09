using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProductQuoteApp.Persistence;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class UserSalesChannelController : BaseController
    {

        private UserManager<ApplicationUser> userManager = null;

        public UserSalesChannelController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        // GET: AdminUser
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Index()
        {
            var users = userManager.Users.OfType<AdminUser>();

            var userList = users.ToList();

            return View(userList);
        }

        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Edit(string userID, string returnUrl)
        {
            var user = userManager.FindById(userID);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.userID = userID;
            ViewBag.userName = user.LastName + ", " + user.FirstName;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }
            base.Dispose(disposing);
        }

    }
}