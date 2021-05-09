using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class SaleModalityCreditRatingApiController : ApiController
    {
        private ISaleModalityRepository saleModalityRepository = null;
        private ISaleModalityCreditRatingRepository saleModalityCreditRatingRepository = null;

        public SaleModalityCreditRatingApiController(ISaleModalityRepository saleModalityRepo, 
            ISaleModalityCreditRatingRepository saleModalityCreditRatingRepo)
        {
            saleModalityRepository = saleModalityRepo;
            saleModalityCreditRatingRepository = saleModalityCreditRatingRepo;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetSaleModalitysMM")]
        public async Task<List<SaleModality>> GetSaleModalitysMM()
        {
            SaleModality sa = null;
            List<SaleModality> result = new List<SaleModality>();
            List<SaleModality> saList = await saleModalityRepository.FindSaleModalitysAsync();

            foreach (SaleModality item in saList)
            {
                sa = new SaleModality();
                sa.SaleModalityID = item.SaleModalityID;
                sa.Description = item.Description;
                result.Add(sa);
                sa = null;
            }
            saList = null;
            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetCreditRatingBySaleModality/{id}")]
        public async Task<List<SaleModalityCreditRatingViewModel>> GetCreditRatingBySaleModality(int id)
        {
            SaleModalityCreditRatingViewModel smcrVM = null;
            List<SaleModalityCreditRatingViewModel> result = new List<SaleModalityCreditRatingViewModel>();
            List<SaleModalityCreditRating> smcrList = await saleModalityCreditRatingRepository.FindSaleModalityCreditRatingsBySaleModalityAsync(id);

            foreach (SaleModalityCreditRating item in smcrList)
            {
                smcrVM = new SaleModalityCreditRatingViewModel(item);

                result.Add(smcrVM);
                smcrVM = null;
            }
            smcrList = null;
            return result;

        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("GetCreditRatingAvailables/{id}")]
        public List<CreditRatingViewModel> GetCreditRatingAvailables(int id)
        {
            List<CreditRatingViewModel> result = new List<CreditRatingViewModel>();
            List<CreditRating> crList = saleModalityCreditRatingRepository.FindCreditRatingAvailableBySaleModality(id);

            foreach (CreditRating item in crList)
            {
                result.Add(new CreditRatingViewModel(item));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("CreateSaleModalityCreditRating")]
        public SaleModalityCreditRating CreateSaleModalityCreditRating(SaleModalityCreditRating saleModalityCreditRating)
        {
            saleModalityCreditRatingRepository.Create(saleModalityCreditRating);

            return saleModalityCreditRating;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("UpdateSaleModalityCreditRatingList")]
        public void UpdateSaleModalityCreditRatingList(List<SaleModalityCreditRating> creditRatingMargensList)
        {
            foreach (SaleModalityCreditRating cr in creditRatingMargensList)
            {
                cr.SaleModality = null;
                cr.CreditRating = null;
                saleModalityCreditRatingRepository.Update(cr);
            }
        }

        [Authorize(Roles = "AdminUser")]
        public HttpResponseMessage DeleteSaleModalityCreditRatingApi(int id)
        {
            try
            {
                saleModalityCreditRatingRepository.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (saleModalityRepository != null)
                { 
                    saleModalityRepository.Dispose();
                    saleModalityRepository = null;
                }
                if (saleModalityCreditRatingRepository != null)
                {
                    saleModalityCreditRatingRepository.Dispose();
                    saleModalityCreditRatingRepository = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}