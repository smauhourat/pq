using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class TransportTypeController : BaseController
    {
        private ITransportTypeRepository transportTypeRepository = null;
        private IFreightTypeRepository freightTypeRepository = null;

        public TransportTypeController(ITransportTypeRepository transportTypeRepo, IFreightTypeRepository freightTypeRepo)
        {
            transportTypeRepository = transportTypeRepo;
            freightTypeRepository = freightTypeRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var transportTypes = await transportTypeRepository.FindTransportTypesAsync();
            return View(transportTypes.ToList());
        }

        // GET: TransportTypes/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            TransportType transportType = await transportTypeRepository.FindTransportTypesByIDAsync(id);
            if (transportType == null)
            {
                return HttpNotFound();
            }
            return View(transportType);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description");
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TransportTypeID,Description,PositionQuantityFrom,PositionQuantityTo,FreightTypeID")] TransportType transportType)
        {
            if (ModelState.IsValid)
            {
                await transportTypeRepository.CreateAsync(transportType);

                return RedirectToAction("Index");
            }

            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", transportType.FreightTypeID);

            return View(transportType);
        }

        // GET: TransportTypes/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            TransportType transportType = await transportTypeRepository.FindTransportTypesByIDAsync(id);
            if (transportType == null)
            {
                return HttpNotFound();
            }

            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", transportType.FreightTypeID);

            return View(transportType);
        }

        // POST: TransportTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TransportTypeID,Description,PositionQuantityFrom,PositionQuantityTo,FreightTypeID")] TransportType transportType)
        {
            if (ModelState.IsValid)
            {
                await transportTypeRepository.UpdateAsync(transportType);
                return RedirectToAction("Index");
            }

            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", transportType.FreightTypeID);

            return View(transportType);
        }

        // GET: TransportTypes/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            TransportType transportType = await transportTypeRepository.FindTransportTypesByIDAsync(id);
            if (transportType == null)
            {
                return HttpNotFound();
            }
            return View(transportType);
        }

        // POST: TransportTypes/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await transportTypeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (transportTypeRepository != null)
                {
                    transportTypeRepository.Dispose();
                    transportTypeRepository = null;
                }
                if (freightTypeRepository != null)
                {
                    freightTypeRepository.Dispose();
                    freightTypeRepository = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}