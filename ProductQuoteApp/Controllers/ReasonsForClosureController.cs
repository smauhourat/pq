using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ReasonsForClosureController : BaseController
    {
        private IGenericRepository<ReasonsForClosure> genericRepository = null;

        public ReasonsForClosureController(IGenericRepository<ReasonsForClosure> genericRepo)
        {
            genericRepository = genericRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var result = await genericRepository.GetAllAsync();
            return View(result.ToList());
        }

        // GET: GeographicAreas/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            ReasonsForClosure entity = await genericRepository.GetByIDAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Create([Bind(Include = "ReasonsForClosureID,Description")] ReasonsForClosure entity)
        {
            if (ModelState.IsValid)
            {
                await genericRepository.CreateAsync(entity);
            }
            return RedirectToAction("Index");
        }

        // GET: IIBBTreatment/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            ReasonsForClosure entity = await genericRepository.GetByIDAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            return View(entity);
        }

        // POST: IIBBTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit([Bind(Include = "ReasonsForClosureID,Description")] ReasonsForClosure entity)
        {
            if (ModelState.IsValid)
            {
                await genericRepository.UpdateAsync(entity);
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        // GET: IIBBTreatment/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            ReasonsForClosure entity = await genericRepository.GetByIDAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: IIBBTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await genericRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && genericRepository != null)
            {
                genericRepository.Dispose();
                genericRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}