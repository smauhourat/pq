using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class GeographicAreaController : BaseController
    {
        private IGeographicAreaRepository geographicAreaRepository = null;

        public GeographicAreaController(IGeographicAreaRepository geographicAreaRepo)
        {
            geographicAreaRepository = geographicAreaRepo;

        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var geographicAreas = await geographicAreaRepository.FindGeographicAreasAsync();
            return View(geographicAreas.ToList());
        }

        // GET: GeographicAreas/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            GeographicArea geographicArea = await geographicAreaRepository.FindGeographicAreasByIDAsync(id);
            if (geographicArea == null)
            {
                return HttpNotFound();
            }
            return View(geographicArea);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GeographicAreaID,Name")] GeographicArea geographicArea)
        {
            if (ModelState.IsValid)
            {
                await geographicAreaRepository.CreateAsync(geographicArea);

                return RedirectToAction("Index");
            }

            return View(geographicArea);
        }

        // GET: GeographicAreas/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            GeographicArea geographicArea = await geographicAreaRepository.FindGeographicAreasByIDAsync(id);
            if (geographicArea == null)
            {
                return HttpNotFound();
            }

            return View(geographicArea);
        }

        // POST: GeographicAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GeographicAreaID,Name")] GeographicArea geographicArea)
        {
            if (ModelState.IsValid)
            {
                await geographicAreaRepository.UpdateAsync(geographicArea);
                return RedirectToAction("Index");
            }

            return View(geographicArea);
        }

        // GET: GeographicAreas/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            GeographicArea geographicArea = await geographicAreaRepository.FindGeographicAreasByIDAsync(id);
            if (geographicArea == null)
            {
                return HttpNotFound();
            }
            return View(geographicArea);
        }

        // POST: GeographicAreas/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await geographicAreaRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && geographicAreaRepository != null)
            {
                geographicAreaRepository.Dispose();
                geographicAreaRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
