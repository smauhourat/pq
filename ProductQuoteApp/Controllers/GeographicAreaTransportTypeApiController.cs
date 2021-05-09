using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class GeographicAreaTransportTypeApiController : ApiController
    {
        private IGeographicAreaRepository geographicAreaRepository = null;
        private IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepository = null;

        public GeographicAreaTransportTypeApiController(IGeographicAreaRepository geographicAreaRepo, 
            IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepo)
        {
            geographicAreaRepository = geographicAreaRepo;
            geographicAreaTransportTypeRepository = geographicAreaTransportTypeRepo;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetGeographicAreasMM")]
        public async Task<List<GeographicArea>> GetGeographicAreasMM()
        {
            GeographicArea ga = null;
            List<GeographicArea> result = new List<GeographicArea>();
            List<GeographicArea> gaList = await geographicAreaRepository.FindGeographicAreasAsync();

            foreach (GeographicArea item in gaList)
            {
                ga = new GeographicArea();
                ga.GeographicAreaID = item.GeographicAreaID;
                ga.Name = item.Name;
                result.Add(ga);
                ga = null;
            }
            gaList = null;
            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetTransportTypeByGeographicArea/{id}")]
        public async Task<List<GeographicAreaTransportTypeViewModel>> GetTransportTypeByGeographicArea(int id)
        {
            GeographicAreaTransportTypeViewModel gattVM = null;
            List<GeographicAreaTransportTypeViewModel> result = new List<GeographicAreaTransportTypeViewModel>();
            List<GeographicAreaTransportType> gattList = await geographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicAreaAsync(id);

            foreach (GeographicAreaTransportType item in gattList)
            {
                gattVM = new GeographicAreaTransportTypeViewModel(item);
                result.Add(gattVM);
                gattVM = null;
            }
            gattList = null;
            return result;
        }

        [Authorize(Roles = "AdminUser")]
        public HttpResponseMessage DeleteGeographicAreaTransportTypeApi(int id)
        {
            try
            {
                geographicAreaTransportTypeRepository.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            
        }


        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("GetTransportTypeAvailables/{id}")]
        public List<TransportTypeViewModel> GetTransportTypeAvailables(int id)
        {
            List<TransportTypeViewModel> result = new List<TransportTypeViewModel>();
            List<Persistence.TransportType> ttList  = geographicAreaTransportTypeRepository.FindTransportTypeAvailableByGeographicArea(id);

            foreach (Persistence.TransportType item in ttList)
            {
                result.Add(new TransportTypeViewModel(item));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("CreateGeographicAreaTransportType")]
        public GeographicAreaTransportType CreateGeographicAreaTransportType(GeographicAreaTransportType geographicAreaTransportType)
        {
            geographicAreaTransportTypeRepository.Create(geographicAreaTransportType);

            return geographicAreaTransportType;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("UpdateGeographicAreaTransportTypeList")]
        public void UpdateGeographicAreaTransportTypeList(List<GeographicAreaTransportType> transportCostList)
        {
            foreach (GeographicAreaTransportType gatt in transportCostList)
            {
                gatt.TransportType = null;
                gatt.GeographicArea = null;
                geographicAreaTransportTypeRepository.Update(gatt);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (geographicAreaRepository != null)
                {
                    geographicAreaRepository.Dispose();
                    geographicAreaRepository = null;
                }
                if (geographicAreaTransportTypeRepository != null)
                {
                    geographicAreaTransportTypeRepository.Dispose();
                    geographicAreaTransportTypeRepository = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}
