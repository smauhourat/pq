using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class PackagingController : BaseController
    {
        private IPackagingRepository packagingRepository = null;

        public PackagingController(IPackagingRepository packagingRepo)
        {
            packagingRepository = packagingRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var packagings = await packagingRepository.FindPackagingsAsync();
            return View(packagings.ToList());
        }

        // GET: Packagings/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            Packaging packaging = await packagingRepository.FindPackagingsByIDAsync(id);
            if (packaging == null)
            {
                return HttpNotFound();
            }
            return View(packaging);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PackagingID,Description,Stackable")] Packaging packaging)
        {
            if (ModelState.IsValid)
            {
                await packagingRepository.CreateAsync(packaging);

                return RedirectToAction("Index");
            }
            return View(packaging);
        }

        // GET: Packagings/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            Packaging packaging = await packagingRepository.FindPackagingsByIDAsync(id);
            if (packaging == null)
            {
                return HttpNotFound();
            }

            return View(packaging);
        }

        // POST: Packagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PackagingID,Description,Stackable")] Packaging packaging)
        {
            if (ModelState.IsValid)
            {
                await packagingRepository.UpdateAsync(packaging);
                return RedirectToAction("Index");
            }

            return View(packaging);
        }

        // GET: Packagings/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            Packaging packaging = await packagingRepository.FindPackagingsByIDAsync(id);
            if (packaging == null)
            {
                return HttpNotFound();
            }
            return View(packaging);
        }

        // POST: Packagings/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await packagingRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && packagingRepository != null)
            {
                packagingRepository.Dispose();
                packagingRepository = null;
            }
            base.Dispose(disposing);
        }

    }
}