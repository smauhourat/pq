using ProductQuoteApp.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class FreightTypeController : BaseController
    {
        private IFreightTypeRepository freightTypeRepository = null;

        public FreightTypeController(IFreightTypeRepository freightTypeRepo)
        {
            freightTypeRepository = freightTypeRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var freightTypes = await freightTypeRepository.FindFreightTypesAsync();
            return View(freightTypes.ToList());
        }

        // GET: FreightTypes/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            FreightType freightType = await freightTypeRepository.FindFreightTypesByIDAsync(id);
            if (freightType == null)
            {
                return HttpNotFound();
            }
            return View(freightType);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FreightTypeID,Description")] FreightType freightType)
        {
            if (ModelState.IsValid)
            {
                await freightTypeRepository.CreateAsync(freightType);

                return RedirectToAction("Index");
            }

            return View(freightType);
        }

        // GET: FreightTypes/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            FreightType freightType = await freightTypeRepository.FindFreightTypesByIDAsync(id);
            if (freightType == null)
            {
                return HttpNotFound();
            }

            return View(freightType);
        }

        // POST: FreightTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FreightTypeID,Description")] FreightType freightType)
        {
            if (ModelState.IsValid)
            {
                await freightTypeRepository.UpdateAsync(freightType);
                return RedirectToAction("Index");
            }

            return View(freightType);
        }

        // GET: FreightTypes/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            FreightType freightType = await freightTypeRepository.FindFreightTypesByIDAsync(id);
            if (freightType == null)
            {
                return HttpNotFound();
            }
            return View(freightType);
        }

        // POST: FreightTypes/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await freightTypeRepository.DeleteAsync(id);
                
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && freightTypeRepository != null)
            {
                freightTypeRepository.Dispose();
                freightTypeRepository = null;
            }
            base.Dispose(disposing);
        }

    }
}