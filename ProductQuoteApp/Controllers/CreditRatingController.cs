using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class CreditRatingController : BaseController
    {
        private ICreditRatingRepository creditRatingRepository = null;

        public CreditRatingController(ICreditRatingRepository creditRatingRepo)
        {
            creditRatingRepository = creditRatingRepo;

        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var geographicAreas = await creditRatingRepository.FindCreditRatingsAsync();
            return View(geographicAreas.ToList());
        }

        // GET: GeographicAreas/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            CreditRating creditRating = await creditRatingRepository.FindCreditRatingByIDAsync(id);
            if (creditRating == null)
            {
                return HttpNotFound();
            }
            return View(creditRating);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Create([Bind(Include = "CreditRatingID,Description")] CreditRating creditRating)
        {
            if (ModelState.IsValid)
            {
                await creditRatingRepository.CreateAsync(creditRating);
            }
            return RedirectToAction("Index");
        }

        // GET: GeographicAreas/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            CreditRating creditRating = await creditRatingRepository.FindCreditRatingByIDAsync(id);
            if (creditRating == null)
            {
                return HttpNotFound();
            }

            return View(creditRating);
        }

        // POST: GeographicAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit([Bind(Include = "CreditRatingID,Description")] CreditRating creditRating)
        {
            if (ModelState.IsValid)
            {
                await creditRatingRepository.UpdateAsync(creditRating);
                return RedirectToAction("Index");
            }

            return View(creditRating);
        }

        // GET: GeographicAreas/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            CreditRating creditRating = await creditRatingRepository.FindCreditRatingByIDAsync(id);
            if (creditRating == null)
            {
                return HttpNotFound();
            }
            return View(creditRating);
        }

        // POST: GeographicAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await creditRatingRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && creditRatingRepository != null)
            {
                creditRatingRepository.Dispose();
                creditRatingRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
