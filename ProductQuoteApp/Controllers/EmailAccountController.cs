using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class EmailAccountController : Controller
    {

        IEmailAccountRepository emailAccountRepository = null;

        public EmailAccountController(IEmailAccountRepository emailAccountRepo)
        {
            emailAccountRepository = emailAccountRepo;
        }

        [Authorize(Roles = "AdminUser")]
        // GET: EmailAccount
        public async Task<ActionResult> Index()
        {
            var emailAccounts = await emailAccountRepository.FindEmailAccountsAsync();
            return View(emailAccounts.ToList());
        }

        // GET: EmailAccount/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            EmailAccount emailAccount = await emailAccountRepository.FindEmailAccountsByIDAsync(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }
            return View(emailAccount);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmailAccountID,Email,DisplayName,Host,Port,Username,Password,EnableSsl,UseDefaultCredentials,IsDefault")] EmailAccount emailAccount)
        {
            if (ModelState.IsValid)
            {
                await emailAccountRepository.CreateAsync(emailAccount);

                return RedirectToAction("Index");
            }
            return View(emailAccount);
        }

        // GET: EmailAccount/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            EmailAccount emailAccount = await emailAccountRepository.FindEmailAccountsByIDAsync(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }

            return View(emailAccount);
        }

        // POST: EmailAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmailAccountID,Email,DisplayName,Host,Port,Username,Password,EnableSsl,UseDefaultCredentials,IsDefault")] EmailAccount emailAccount)
        {
            if (ModelState.IsValid)
            {
                await emailAccountRepository.UpdateAsync(emailAccount);
                return RedirectToAction("Index");
            }

            return View(emailAccount);
        }

        // GET: EmailAccount/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            EmailAccount emailAccount = await emailAccountRepository.FindEmailAccountsByIDAsync(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }
            return View(emailAccount);
        }

        // POST: EmailAccount/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await emailAccountRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && emailAccountRepository != null)
            {
                emailAccountRepository.Dispose();
                emailAccountRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}