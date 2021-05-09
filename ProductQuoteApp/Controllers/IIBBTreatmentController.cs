using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class IIBBTreatmentController : BaseController
    {
        private IGenericRepository<IIBBTreatment> genericRepository = null;

        public IIBBTreatmentController(IGenericRepository<IIBBTreatment> genericRepo)
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
            IIBBTreatment iibbTreatment = await genericRepository.GetByIDAsync(id);
            if (iibbTreatment == null)
            {
                return HttpNotFound();
            }
            return View(iibbTreatment);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Create([Bind(Include = "IIBBTreatmentID,Description,Percentage")] IIBBTreatment iibbTreatment)
        {
            if (ModelState.IsValid)
            {
                await genericRepository.CreateAsync(iibbTreatment);
            }
            return RedirectToAction("Index");
        }

        // GET: IIBBTreatment/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            IIBBTreatment iibbTreatment = await genericRepository.GetByIDAsync(id);
            if (iibbTreatment == null)
            {
                return HttpNotFound();
            }

            return View(iibbTreatment);
        }

        // POST: IIBBTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit([Bind(Include = "IIBBTreatmentID,Description,Percentage")] IIBBTreatment iibbTreatment)
        {
            if (ModelState.IsValid)
            {
                await genericRepository.UpdateAsync(iibbTreatment);
                return RedirectToAction("Index");
            }

            return View(iibbTreatment);
        }

        // GET: IIBBTreatment/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            IIBBTreatment iibbTreatment = await genericRepository.GetByIDAsync(id);
            if (iibbTreatment == null)
            {
                return HttpNotFound();
            }
            return View(iibbTreatment);
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