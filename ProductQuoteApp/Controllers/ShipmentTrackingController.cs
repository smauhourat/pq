using ProductQuoteApp.Persistence;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ShipmentTrackingController : BaseController
    {
        private IShipmentTrackingRepository shipmentTrackingRepository = null;

        public ShipmentTrackingController(IShipmentTrackingRepository shipmentTrackingRepo)
        {
            shipmentTrackingRepository = shipmentTrackingRepo;
        }

        // GET: ShipmentTrackings/Edit/5
        [Authorize(Roles = "AdminUser")]
        public ActionResult Edit(int pq, int customerID, int id)
        {
            ShipmentTracking shipmentTracking = shipmentTrackingRepository.FindShipmentTrackingByProductQuoteID(id);

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (shipmentTracking == null)
            {
                return HttpNotFound();
            }

            return View(shipmentTracking);
        }

        // POST: ShipmentTrackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int pq, int customerID, [Bind(Include = "ProductQuoteID, QuotedEstimatedDate, QuotedRealDate, QuotedCompleted, CustomerOrderEstimatedDate, CustomerOrderRealDate, CustomerOrderCompleted, ApprovedEstimatedDate, ApprovedRealDate, ApprovedCompleted, InProductionEstimatedDate, InProductionRealDate, InProductionCompleted, ETDEstimatedDate, ETDRealDate, ETDCompleted, ETAEstimatedDate, ETARealDate, ETACompleted, NationalizedEstimatedDate, NationalizedRealDate, NationalizedCompleted, DeliveredEstimatedDate, DeliveredRealDate, DeliveredCompleted, InProductionEnabled, ETDEnabled, ETAEnabled, NationalizedEnabled, DeliveredEnabled")] ShipmentTracking shipmentTracking)
        {
            if (ModelState.IsValid)
            {
                await shipmentTrackingRepository.UpdateAsync(shipmentTracking);
                return RedirectToAction("../ProductQuote", new { pq = pq, customerID = customerID });
            }

            return View(shipmentTracking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && shipmentTrackingRepository != null)
            {
                shipmentTrackingRepository.Dispose();
                shipmentTrackingRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}