using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class PaymentDeadlineController : BaseController
    {
        private IPaymentDeadlineRepository paymentDeadlineRepository = null;

        public PaymentDeadlineController(IPaymentDeadlineRepository paymentDeadlineRepo)
        {
            paymentDeadlineRepository = paymentDeadlineRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var result = await paymentDeadlineRepository.FindPaymentDeadlinesAsync();
            return View(result.ToList());
        }

        // GET: Packagings/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            PaymentDeadline paymentDeadline = await paymentDeadlineRepository.FindPaymentDeadlineByIDAsync(id);
            if (paymentDeadline == null)
            {
                return HttpNotFound();
            }
            return View(paymentDeadline);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PaymentDeadlineID,Description,Days")] PaymentDeadline paymentDeadline)
        {
            if (ModelState.IsValid)
            {
                await paymentDeadlineRepository.CreateAsync(paymentDeadline);

                return RedirectToAction("Index");
            }

            return View(paymentDeadline);
        }

        // GET: Packagings/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            PaymentDeadline paymentDeadline = await paymentDeadlineRepository.FindPaymentDeadlineByIDAsync(id);
            if (paymentDeadline == null)
            {
                return HttpNotFound();
            }

            return View(paymentDeadline);
        }

        // POST: Packagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PaymentDeadlineID,Description,Days")] PaymentDeadline paymentDeadline)
        {
            if (ModelState.IsValid)
            {
                await paymentDeadlineRepository.UpdateAsync(paymentDeadline);
                return RedirectToAction("Index");
            }

            return View(paymentDeadline);
        }

        // GET: Packagings/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            PaymentDeadline paymentDeadline = await paymentDeadlineRepository.FindPaymentDeadlineByIDAsync(id);
            if (paymentDeadline == null)
            {
                return HttpNotFound();
            }
            return View(paymentDeadline);
        }

        // POST: Packagings/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await paymentDeadlineRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && paymentDeadlineRepository != null)
            {
                paymentDeadlineRepository.Dispose();
                paymentDeadlineRepository = null;
            }
            base.Dispose(disposing);
        }

    }
}