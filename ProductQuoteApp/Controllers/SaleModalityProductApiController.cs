using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class SaleModalityProductApiController : ApiController
    {
        private ISaleModalityRepository saleModalityRepository = null;
        private ISaleModalityProductRepository saleModalityProductRepository = null;

        public SaleModalityProductApiController(ISaleModalityRepository saleModalityRepo, ISaleModalityProductRepository saleModalityProductRepo)
        {
            saleModalityRepository = saleModalityRepo;
            saleModalityProductRepository = saleModalityProductRepo;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetSaleModalitysMM2")]
        public async Task<List<SaleModality>> GetSaleModalitysMM2()
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
        [Route("GetProductBySaleModality/{id}")]
        public async Task<List<SaleModalityProductViewModel>> GetProductBySaleModality(int id)
        {
            SaleModalityProductViewModel smpVM = null;
            List<SaleModalityProductViewModel> result = new List<SaleModalityProductViewModel>();
            List<SaleModalityProduct> smpList = await saleModalityProductRepository.FindSaleModalityProductsBySaleModalityAsync(id);
            foreach (SaleModalityProduct item in smpList)
            {
                smpVM = new Models.SaleModalityProductViewModel(item);
                result.Add(smpVM);
                smpVM = null;
            }
            smpList = null;
            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("GetProductAvailables/{id}")]
        public List<ProductSingleViewModel> GetProductAvailables(int id)
        {
            ProductSingleViewModel prodVM = null;
            List<ProductSingleViewModel> result = new List<ProductSingleViewModel>();
            List<Product> prodList = saleModalityProductRepository.FindProductAvailableBySaleModality(id);

            foreach (Product item in prodList)
            {
                prodVM = new ProductSingleViewModel(item);
                result.Add(prodVM);
                prodVM = null;
            }
            prodList = null;
            return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("CreateSaleModalityProduct")]
        public SaleModalityProduct CreateSaleModalityProduct(SaleModalityProduct saleModalityProduct)
        {
            saleModalityProductRepository.Create(saleModalityProduct);

            return saleModalityProduct;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("UpdateSaleModalityProductList")]
        public void UpdateSaleModalityProductList(List<SaleModalityProduct> saleModalityProductList)
        {
            foreach (SaleModalityProduct smp in saleModalityProductList)
            {
                smp.SaleModality = null;
                smp.Product = null;
                saleModalityProductRepository.Update(smp);
            }
        }

        [Authorize(Roles = "AdminUser")]
        public HttpResponseMessage DeleteSaleModalityProductApi(int id)
        {
            try
            {
                saleModalityProductRepository.Delete(id);

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
                if (saleModalityProductRepository != null)
                {
                    saleModalityProductRepository.Dispose();
                    saleModalityProductRepository = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}