using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class CustomerProductApiController : ApiController
    {
        private ICustomerProductRepository customerProductRepository = null;

        public CustomerProductApiController(ICustomerProductRepository customerProductRepo)
        {
            customerProductRepository = customerProductRepo;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetAssignedProductsApi/{customerID}")]
        public async Task<List<CustomerProductSingleViewModel>> GetAssignedProductsApi(int customerID)
        {
            List<CustomerProductSingleViewModel> cpList = new List<CustomerProductSingleViewModel>();
            IEnumerable<CustomerProduct> result = await customerProductRepository.FindCustomerProductsByCustomerIDAsync(customerID);
            result = result.OrderBy(s => s.Product.FullName.ToUpper());

            foreach (CustomerProduct item in result)
            {
                cpList.Add(new Models.CustomerProductSingleViewModel(item));
            }
            return cpList;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetAvailableProductsApi/{customerID}")]
        public async Task<List<ProductSingleViewModel>> GetAvailableProductsApi(int customerID)
        {
            List<ProductSingleViewModel> cpList = new List<ProductSingleViewModel>();
            IEnumerable<Product> result = await customerProductRepository.FindProductsAvailableByCustomerAsync(customerID);
            result = result.OrderBy(s => s.FullName.ToUpper());

            foreach (Product item in result)
            {
                cpList.Add(new ProductSingleViewModel(item));
            }

            return cpList;
        }

        //changeCustomerProductCalDetailsValue
        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("ChangeCustomerProductCalDetailsValue")]
        public void ChangeCustomerProductCalDetailsValue(CustomerProductSingleViewModel model)
        {
            CustomerProduct cp = new CustomerProduct();
            cp.CustomerProductID = model.CustomerProductID;
            cp.CustomerID = model.CustomerID;
            cp.ProductID = model.ProductID;
            cp.CalculationDetails = model.CalculationDetails;

            customerProductRepository.Update(cp);
        }

        //AddProductToCustomer
        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("AddProductToCustomer/{customerID}")]
        public void AddProductToCustomer(ProductSingleViewModel model, int customerID)
        {
            CustomerProduct cp = new CustomerProduct();
            cp.CustomerID = customerID;
            cp.ProductID = model.ProductID;
            cp.CalculationDetails = false;

            customerProductRepository.Create(cp);
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("DelAllCustomerProductsByCustomer/{customerID}")]
        public void DelAllCustomerProductsByCustomer(int customerID)
        {
            customerProductRepository.DeleteByCustomer(customerID);
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("AddAllProductsToCustomer/{customerID}")]
        public void AddAllProductsToCustomer(int customerID)
        {
            customerProductRepository.AddAllProductsToCustomer(customerID);
        }

        [Authorize(Roles = "AdminUser")]
        public HttpResponseMessage DeleteCustomerProductApi(int id)
        {
            try
            {
                customerProductRepository.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && customerProductRepository != null)
            {
                customerProductRepository.Dispose();
                customerProductRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}