using Newtonsoft.Json;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class ShipmentTrackingApiController: ApiController
    {
        private IShipmentTrackingRepository shipmentTrackingRepository = null;

        public ShipmentTrackingApiController(IShipmentTrackingRepository shipmentTrackingRepo)
        {
            shipmentTrackingRepository = shipmentTrackingRepo;
        }

        [System.Web.Http.Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetShipmentTracking")]
        public String GetShipmentTracking(int Id)
        {
            ShipmentTracking result = shipmentTrackingRepository.FindShipmentTrackingByProductQuoteID(Id);

            return JsonConvert.SerializeObject(new ShipmentTrackingViewModel(result));
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
